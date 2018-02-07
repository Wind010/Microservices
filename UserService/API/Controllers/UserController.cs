
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Services.User.Controllers
{
    using Domain = Processors.Models.Domain;
    using Models.Rest;
    using Models.Rest.Response;
    using Models.Rest.Request;
    using Processors;

    using AutoMapper;
    using RawRabbit;
    using RawRabbit.Logging;
    using Newtonsoft.Json;
    using RawRabbit.Configuration.Exchange;

    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserProcessor _userProcessor;
        private static IBusClient _busClient;
        private static ILogger _logger;

        private readonly string _exchangeName;
        private readonly string _incomingQueue;
        private readonly string _outgoingQueue;
        private readonly string _incomingRoutingKey;
        private readonly string _outgoingRoutingKey;

        public UserController(IUserProcessor userProcessor, IBusClient busClient, IConfiguration config, ILoggerFactory loggerFactory)
        {
            _userProcessor = userProcessor;
            _logger = loggerFactory.CreateLogger<UserController>();

            Mapper.Initialize(cfg => {
                cfg.CreateMap<Domain.User, UserRequest>().ReverseMap();
                cfg.CreateMap<Domain.Address, Address>().ReverseMap();
                cfg.CreateMap<Domain.ContactType, ContactType>().ReverseMap();
                cfg.CreateMap<Domain.ContactDetail, ContactDetail>().ReverseMap();
                cfg.CreateMap<Domain.StateProvince, StateProvince>().ReverseMap();
                cfg.CreateMap<Domain.Preference, Preference>().ReverseMap();
            });

            _exchangeName = config.GetValue<string>("RabbitMQ:Exchanges:0:Name");

            _incomingQueue = config.GetValue<string>($"RabbitMQ:Queues:0:Name");
            _outgoingQueue = config.GetValue<string>($"RabbitMQ:Queues:1:Name");

            _incomingRoutingKey = config.GetValue<string>($"RabbitMQ:Queues:0:RoutingKey");
            _outgoingRoutingKey = config.GetValue<string>($"RabbitMQ:Queues:1:RoutingKey");


            InitializeBusClient(busClient);
        }

        private void InitializeBusClient(IBusClient busClient)
        {
            if (_busClient != null)
            {
                return;
            }

            _busClient = busClient;

            // Default RawRabbit configuration will create new queues for each request.  Not what we want.
            Subscribe(_exchangeName, _incomingQueue, _incomingRoutingKey);
        }


        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "User", "McUserFace" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<UserResponse> Get(int id)
        {
            _logger.LogDebug($"{nameof(Get)}:  Id = {id}.");
            Domain.User domainUser = await _userProcessor.GetUserById(id);
            var userResponse = Mapper.Map<UserResponse>(domainUser);
            return userResponse;
        }
        
        // POST api/values
        [HttpPost]
        public async Task<int> Post([FromBody] UserRequest userRequest)
        {
            if (userRequest == null)
            {
                throw new Exception(Constants.UnableToDeserializeRequest);
            }

            string userJson = JsonConvert.SerializeObject(userRequest);
            _logger.LogDebug($"Received insert request:  {userJson}");

            var user = Mapper.Map<Domain.User>(userRequest);

            int userId = await AddUserToDatabase(user);

            if (userId > 0)
            {
                await PublishAsync(userRequest, _exchangeName, _outgoingRoutingKey);
            }

            return userId;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(Guid userId, [FromBody]UserRequest userRequest)
        {
            //string userJson = JsonConvert.SerializeObject(userRequest);
            //_logger.LogDebug($"Received update request:  {userJson}");

            //var domainUser = Mapper.Map<Domain.User>(userRequest);

            ////int rowsUpdated = await _UserProcessor.(domainUser);

            //return Ok(new UpdateUserResponse(userRequest.TransactionId.Value, userId));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        internal async Task<int> AddUserToDatabase(Domain.User user)
        {
            string UserJson = JsonConvert.SerializeObject(user);
            _logger.LogDebug($"Received from Queue:  {UserJson}");

            Domain.User domainUser = Mapper.Map<Domain.User>(user);
            int userId = await _userProcessor.AddUser(domainUser);

            return userId;
        }


        private async Task PublishAsync(UserRequest userRequest, string exchangeName, string routingKey)
        {
            await _busClient.PublishAsync(userRequest, Guid.NewGuid(),
                cfg => cfg
            .WithExchange(exchange =>
                exchange
                  .WithType(ExchangeType.Topic)
                  .WithAutoDelete()
                  .WithName(exchangeName)
                )
            .WithRoutingKey(routingKey)
            );
        }


        /// <summary>
        /// Since one instance is on the VisualOne side and one on the Seat side, the 
        /// adding of a User is 
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="queueName"></param>
        /// <param name="routingKey"></param>
        private void Subscribe(string exchangeName, string queueName, string routingKey)
        {
            // Publish/Subscribe or Request/Reply pattern up for discussion.
            _busClient.SubscribeAsync<UserRequest>(async (msg, context) =>
            {
                Domain.User domainUser = Mapper.Map<Domain.User>(msg);
                await AddUserToDatabase(domainUser);
            }, cfg => cfg
            .WithExchange(e => e
                  .WithType(ExchangeType.Topic)
                  .WithAutoDelete()
                  .WithName(exchangeName)
                )
            .WithQueue(q => q
                .WithName(queueName)
                .WithExclusivity(false)
                //.WithArgument(QueueArgument.QueueMode, "lazy")
                )
            .WithRoutingKey(routingKey)
            .WithSubscriberId("")
            );
        }

    }
}
