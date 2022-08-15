using System;
using Nuclio.Sdk;

// @nuclio.configure
//
// function.yaml
//   spec:
//     runtime: dotnetcore
//     handler: Function:execute
public class function
{
    public object execute(Context context, Event eventBase)
    {
        var body = eventBase.GetBody().ToString();
        return new Response()
        {
            StatusCode = 200,
            ContentType = "application/text",
            Body = "Hello! I executed! You sent the following body contents:" + body
        };
    }
}