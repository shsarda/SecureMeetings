using AdaptiveCards;
using MeetingEventsCallingBot.Constants;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MeetingEventsCallingBot.Services
{
    public static class NotificationService
    { 
        /// <summary>
        /// Send content bubble to a participant in the meeting.
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="tenantId"></param>
        public static async Task PostStatusChangeNotification(
            ITurnContext<IEventActivity> turnContext,
            string teamsAppId,
            string contentBubbleUrl,
            CancellationToken cancellationToken)
        {
            var activity = new Activity
            {
                Type = ActivityTypes.Message,
                Text = Strings.NotificationTitle,
                ChannelData = new TeamsChannelData
                {
                    Notification = new NotificationInfo
                    {
                        // AlertInMeeting = true and the ExternalResourceUrl in this format cause a content bubble notification to be sent
                        AlertInMeeting = true,
                        ExternalResourceUrl = $"https://teams.microsoft.com/l/bubble/{teamsAppId}?url={contentBubbleUrl}&height=150&width=350&title=Security Notification!",
                    },
                },
            };

            await turnContext.SendActivityAsync(activity, cancellationToken);
        }

        public static Attachment GetNotificationCard(string notificationText)
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Text = notificationText,
                        Wrap = true,
                        HorizontalAlignment = AdaptiveHorizontalAlignment.Left,
                    },
                },
            };

            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card,
            };
        }
    }
}
