using System;
using Nuclio.Sdk;
using NATS.Client;
using Newtonsoft.Json;

// @nuclio.configure
//
// function.yaml
//   spec:
//     runtime: dotnetcore
//     handler: Function:execute
public class function
{
    IConnection nc;

    public object execute(Context context, Event eventBase)
    {
        try
        {
            context.Logger.Info("Invoking function");
            var s = JsonConvert.SerializeObject(eventBase, Formatting.Indented);
            //  context.Logger.Info(s);

            var body = eventBase.Body;

            // we will assume two types of triggers that we handle:
            // nats and http

            context.Logger.Info("Trigger Kind: " + eventBase.Trigger.Kind);

            Msg returnMsg = new Msg();
            returnMsg.Subject = "i.j.k.l";
            //  returnMsg.Data = System.Text.Encoding.UTF8.GetBytes(eventBase.GetBody());
            //   nc.Publish(returnMsg);

            if(eventBase.Trigger.Kind == "http")
            {
                return new Response()
                {
                    StatusCode = 200,
                    ContentType = "application/text",
                    Body = s//JsonConvert.SerializeObject(eventBase)
                };
            }
            else
            {
                context.Logger.Info(
                    "{" +
                    "   StatusCode=200," +
                    "   ContentType='application/text',"  + 
                    "   Body='nats:' " + System.Text.Encoding.UTF8.GetString(body)
                    );
                //todo: de-serialize into a messagewrapper and check the type -- note that we would only expect DataUpdates here because we are only listening to data topics
                return new Response()
                {
                    StatusCode = 200,
                    ContentType = "application/text",
                    Body = "nats: " + System.Text.Encoding.UTF8.GetString(body)
                };
            }
           
        }
        catch (Exception e)
        {
            context.Logger.Error(e.Message + "\n" + e.StackTrace);
            return new Response()
            {
                StatusCode = 500,
                ContentType = "application/text",
                Body = e.Message + "\n" + e.StackTrace
            };
        }
    }

    public void init()
    {
        ConnectionFactory cf = new ConnectionFactory();
        nc = cf.CreateConnection("nats://10.39.83.252:4222");
    }

    
}

