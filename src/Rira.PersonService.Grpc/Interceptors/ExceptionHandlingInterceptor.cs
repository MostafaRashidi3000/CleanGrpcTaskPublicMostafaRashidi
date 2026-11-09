using Grpc.Core;
using Grpc.Core.Interceptors;
namespace Rira.PersonService.Grpc.Interceptors;
public sealed class ExceptionHandlingInterceptor(ILogger<ExceptionHandlingInterceptor> logger):Interceptor
{
 public override async Task<TResponse> UnaryServerHandler<TRequest,TResponse>(TRequest request,ServerCallContext context,UnaryServerMethod<TRequest,TResponse> continuation)
 {try{return await continuation(request,context);}catch(RpcException){throw;}catch(OperationCanceledException) when(context.CancellationToken.IsCancellationRequested){throw new RpcException(new Status(StatusCode.Cancelled,"Request was cancelled."));}catch(Exception ex){logger.LogError(ex,"Unhandled gRPC request failure; TraceId={TraceId}",context.GetHttpContext().TraceIdentifier);throw new RpcException(new Status(StatusCode.Internal,"An unexpected server error occurred."));}}
}
