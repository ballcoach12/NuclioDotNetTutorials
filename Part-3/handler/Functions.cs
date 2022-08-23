using DatabusMessages;
using NATS.Client;
using Nuclio.Sdk;
using System;

//using Newtonsoft.Json;

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
        try
        {
            string natsUrl = "10.202.26.169";
            string natsPort = "9012";
            if (Environment.GetEnvironmentVariable("NATS_URL") != null)
            {
                natsUrl = Environment.GetEnvironmentVariable("NATS_URL");
            }

            if (Environment.GetEnvironmentVariable("NATS_PORT") != null)
            {
                natsPort = Environment.GetEnvironmentVariable("NATS_PORT");
            }

            string url = "nats://" + natsUrl + ":" + natsPort;

            context.Logger.Info(url);


            context.Logger.Info("Invoking function");

            var body = eventBase.Body;

            context.Logger.Info("Trigger Kind: " + eventBase.Trigger.Kind);

            // if it's nats, then de-serialize the message wrapper
            if (eventBase.Trigger.Kind == "nats")
            {
                ProtobufEncoder encoder = new ProtobufEncoder();
                var wrapper = encoder.Decode<MessageWrapper>(eventBase.Body);
                switch (wrapper.Type)
                {
                    case MessageType.DataChanged:
                        {
                            var data = encoder.Decode<DataChangedMessage>(wrapper.Data);
                            switch (data.itemType)
                            {
                                case DataMapItemType.Analog:
                                    {
                                        //divide the value by 10 and re-publish
                                        context.Logger.Info("Received data update for topic '" + data.Topic + "'.");
                                        var value = encoder.Decode<AnalogDataMapItem>(data.Data);

                                        if (!value.isForced)
                                        {
                                            var returnValue = new AnalogDataMapItem
                                            {
                                                Classification = value.Classification,
                                                Index = value.Index,
                                                Quality = value.Quality,
                                                Timestamp = value.Timestamp,
                                                Alias = data.Topic + ".scaled",
                                                Value = value.Value / 10
                                            };

                                            DataChangedMessage returnMsg = new DataChangedMessage
                                            {
                                                Data = encoder.Encode(MessageType.DataChanged, returnValue.Alias, returnValue),
                                                itemType = DataMapItemType.Analog,
                                                Topic = returnValue.Alias
                                            };

                                            var bytes = encoder.Encode(returnMsg);

                                            try
                                            {
                                                context.Logger.Info("Nats address: " + url);
                                                context.Logger.Info("Subject: " + returnMsg.Topic);
                                                ConnectionFactory cf = new ConnectionFactory(); // todo: this should be persisted and re-loaded -- maybe the nats connection too
                                                IConnection nc = cf.CreateConnection(url);
                                                nc.Publish(returnMsg.Topic, bytes);
                                            }
                                            catch (Exception e)
                                            {
                                                context.Logger.Error("Error publishing response to Nats: " + e.Message + "\n" + e.StackTrace);
                                            }
                                        }

                                        break;
                                    }
                                default:
                                    {
                                        context.Logger.Warning(
                                            "The Scale() function received a non-numeric value; only numeric values can be accepted. Please check your Triggers to ensure that the topic name represents an analog value.");
                                        break;
                                    }
                            }
                            break;
                        }
                    case MessageType.TopicListRequest:
                        {
                            //todo: return a response for the topic that we know about
                            break;
                        }
                    default:
                        {
                            // do nothing -- we should never get here because we don't support any other messages
                            context.Logger.Warning("Unexpected message type from Nats: " + wrapper.Type.ToString() + ". Check your Trigger configuration to ensure that you have subscribed to the correct topic.");
                            break;
                        }
                }

            }

            return new Response()
            {
                StatusCode = 200,
                ContentType = "application/text",
                Body = "http: " + eventBase.GetBody() // todo: assume that we got a json data changed message; json-ify the result in another DCM
            };

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



}

