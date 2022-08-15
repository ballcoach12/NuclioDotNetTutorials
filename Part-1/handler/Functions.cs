using System;
using Nuclio.Sdk;

public class Function
{
    public object execute(Context context, Event eventBase)
    {
        return new Response()
        {
            StatusCode = 200,
            ContentType = "application/text",
            Body = ""
        };
    }
}