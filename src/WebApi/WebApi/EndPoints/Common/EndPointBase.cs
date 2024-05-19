using Microsoft.AspNetCore.Mvc;

namespace WebApi.EndPoints.Common;

///<summary>
/// Defines API endpoints with various request and response configurations.
///</summary>
public static class ApiEndpoint {
    
    ///<summary>
    /// Defines API endpoints with a request.
    ///</summary>
    public static class WithRequest<TRequest> {
        
        ///<summary>
        /// Defines API endpoints with a request and a response.
        ///</summary>
        public abstract class WithResponse<TResponse> : EndpointBase {
            
            ///<summary>
            /// Handles the API endpoint asynchronously with the specified request.
            ///</summary>
            ///<param name="request">The request data.</param>
            ///<returns>An asynchronous task that represents the operation and contains the action result with the response data.</returns>
            public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request);
        }

        ///<summary>
        /// Defines API endpoints with a request but without a response.
        ///</summary>
        public abstract class WithoutResponse : EndpointBase {
            
            ///<summary>
            /// Handles the API endpoint asynchronously with the specified request.
            ///</summary>
            ///<param name="request">The request data.</param>
            ///<returns>An asynchronous task that represents the operation and contains the action result.</returns>
            public abstract Task<ActionResult> HandleAsync(TRequest request);
        }
    }

    ///<summary>
    /// Defines API endpoints without a request.
    ///</summary>
    public static class WithoutRequest {
        
        ///<summary>
        /// Defines API endpoints without a request but with a response.
        ///</summary>
        public abstract class WithResponse<TResponse> : EndpointBase {
            
            ///<summary>
            /// Handles the API endpoint asynchronously.
            ///</summary>
            ///<returns>An asynchronous task that represents the operation and contains the action result with the response data.</returns>
            public abstract Task<ActionResult<TResponse>> HandleAsync();
        }

        ///<summary>
        /// Defines API endpoints without a request and without a response.
        ///</summary>
        public abstract class WithoutResponse : EndpointBase {
            
            ///<summary>
            /// Handles the API endpoint asynchronously.
            ///</summary>
            ///<returns>An asynchronous task that represents the operation and contains the action result.</returns>
            public abstract Task<ActionResult> HandleAsync();
        }
    }
}

///<summary>
/// Base class for API endpoints.
///</summary>
[ApiController]
[Route("api")]
public abstract class EndpointBase : ControllerBase;