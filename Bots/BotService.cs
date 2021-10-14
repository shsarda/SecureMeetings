// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.13.2

using MeetingEventsCallingBot.Constants;
using MeetingEventsCallingBot.Model;
using MeetingEventsCallingBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MeetingEventsCallingBot.Bots
{
    public class BotService : ActivityHandler
    {
        private readonly BotOptions options;

        public BotService(IOptions<BotOptions> botOptions)
        {
            this.options = botOptions.Value; 
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyText = $"Echo: {turnContext.Activity.Text}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnEventActivityAsync(ITurnContext<IEventActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Type == "event")
            {
                var meetingEventInfo = turnContext.Activity.Value as JObject;
                var meetingEventInfoObject = meetingEventInfo.ToObject<MeetingStartEndEventValue>();

                if (meetingEventInfoObject.StartTime != null)
                {                     
                    string meetingId = turnContext.Activity.ChannelData?.meeting?.id;

                    if (meetingId != null)
                    {
                        await NotificationService.PostStatusChangeNotification(
                            turnContext,
                            this.options.AppId,
                            this.options.ContentBubbleUrl,
                            cancellationToken);
                    }

                    await turnContext.SendActivityAsync(MessageFactory.Attachment(NotificationService.GetNotificationCard(Strings.MeetingStartNotification)), cancellationToken);
                } 
                else if (meetingEventInfoObject.EndTime != null)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Attachment(NotificationService.GetNotificationCard(Strings.MeetingEndNotification)), cancellationToken);
                }
            }
        }
    }
}
