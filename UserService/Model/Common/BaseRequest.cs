//----------------------------------------------------------------------------------------------------------------------
// <summary>
//     The base class for all gateway Web API requests.
// </summary>
//----------------------------------------------------------------------------------------------------------------------


using System;

namespace Services.User.Models.Rest
{
    using Newtonsoft.Json;


    /// <summary>
    /// The base class for all Web API requests.
    /// </summary>
    [Serializable]
    public abstract class BaseRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRequest"/> class
        /// </summary>
        protected BaseRequest()
        {
        }

        #endregion Constructors


        #region Public Properties

        /// <summary>
        /// The request identifier, if any. 
        /// This is an optional client-generated GUID that may be logged or stored with the request
        /// </summary>
        /// <value>
        /// The request identifier.
        /// </value>
        [JsonProperty(Order = 0)]
        public Guid? TransactionId { get; set; }

        #endregion Public Properties

    }


}