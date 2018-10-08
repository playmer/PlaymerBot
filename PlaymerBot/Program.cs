using System;
using System.IO;
using System.Net;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

using TwitchLib.Api;
using TwitchLib.Api.Models.Helix.Users.GetUsersFollows;
using TwitchLib.Api.Models.v5.Subscriptions;
using System.Text.RegularExpressions;

namespace PlaymerBot
{
    struct Secret
    {
        static public Secret ReadSecret()
        {
            Secret secret;

            var lines = File.ReadAllLines("secret.config");

            secret.mUsername = lines[0];
            secret.mTwitchOAuth = lines[1];

            return secret;
        }

        public string mUsername;
        public string mTwitchOAuth;
    }


    struct Server
    {
        static public Server ReadServer()
        {
            Server server;

            var lines = File.ReadAllLines("server.config");

            server.mChannel = lines[0];

            return server;
        }

        public string mChannel;
    }

    class Program
    {
        //private async Task ExampleCallsAsync()
        //{
        //    //Checks subscription for a specific user and the channel specified.
        //    Subscription subscription = await api.Channels.v5.CheckChannelSubscriptionByUserAsync("channel_id", "user_id");
        //
        //    //Gets a list of all the subscriptions of the specified channel.
        //    List<Subscription> allSubscriptions = await api.Channels.v5.GetAllSubscribersAsync("channel_id");
        //
        //    //Get channels a specified user follows.
        //    GetUsersFollowsResponse userFollows = await api.Users.helix.GetUsersFollowsAsync("user_id");
        //
        //    //Get Specified Channel Follows
        //    var channelFollowers = await api.Channels.v5.GetChannelFollowersAsync("channel_id");
        //
        //    //Return bool if channel is online/offline.
        //    bool isStreaming = await api.Streams.v5.BroadcasterOnlineAsync("channel_id");
        //
        //    //Update Channel Title/Game
        //    await api.Channels.v5.UpdateChannelAsync("channel_id", "New stream title", "Stronghold Crusader");
        //
        //    var emoticons = await api.Chat.v5.GetChatEmoticonsBySetAsync();
        //}

        static void Main(string[] args)
        {
            Bot bot = new Bot();
            Console.ReadLine();
        }
    }

    class Bot
    {
        TwitchAPI api;
        TwitchClient client;
        //List<string> emoteNames;

        public Bot()
        {
            //emoteNames = new List<string>();
            //api = new TwitchAPI();
            //
            //api.Settings.ClientId = "cpg9olp6khldi8w8cvmxxlnel6pqb5";
            //api.Settings.AccessToken = "o2t8u4w1uzsnwvwyo7kfnkrqxminp2";
            //
            //var emoticons = api.Chat.v5.GetChatEmoticonsBySetAsync().Result;
            //
            //foreach (var emote in emoticons.Emoticons)
            //{
            //    emoteNames.Add(emote.Code);
            //}

            var secret = Secret.ReadSecret();
            var server = Server.ReadServer();

            ConnectionCredentials credentials = new ConnectionCredentials(secret.mUsername, secret.mTwitchOAuth);

            client = new TwitchClient();
            client.Initialize(credentials, server.mChannel);
            
            client.OnUserJoined += onUserJoinedChannel;
            client.OnJoinedChannel += onJoinedChannel;
            client.OnMessageReceived += onMessageReceived;
            client.OnWhisperReceived += onWhisperReceived;
            client.OnNewSubscriber += onNewSubscriber;
            client.OnConnected += Client_OnConnected;
            client.OnNewSubscriber += onNewSubscriber;
            client.OnReSubscriber += onReSubscriber;
            client.OnGiftedSubscription += onGiftedSubscription;

            client.Connect();

            //var channel = api.Streams.v5.GetStreamByUserAsync("amytfalcone").Result;
            //api.Streams.v5.GetUptimeAsync("amytfalcone").Result

            //Console.WriteLine("Uptime:");
            //Console.WriteLine(channel.Stream.CreatedAt);
        }


    
        private void onUserJoinedChannel(object sender, OnUserJoinedArgs e)
        {
            Console.WriteLine("User joined: " + e.Username);
            //client.SendMessage(e.Channel, "Hey guys! I am a bot connected via TwitchLib!");
        }

        private void MessageChannel(JoinedChannel aChannel, string aMessage)
        {
            //client.SendMessage(aChannel, $"PlaymerBot:  {aMessage}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void onJoinedChannel(object sender, OnJoinedChannelArgs e)
        {

            //client.SendMessage(e.Channel, "Hey guys! I am a bot connected via TwitchLib!");
        }


        //public event EventHandler<OnNewSubscriberArgs> OnNewSubscriber;
        //public event EventHandler<OnReSubscriberArgs> OnReSubscriber;
        //public event EventHandler<OnGiftedSubscriptionArgs> OnGiftedSubscription;
        //public event EventHandler<OnBeingHostedArgs> OnBeingHosted;


        private void onNewSubscriber(object sender, OnNewSubscriberArgs e)
        {

            if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
            {
                //client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
            }
            else
            {
                //client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
            }

            //e.Subscriber
            SubHype();
        }

        private void onReSubscriber(object sender, OnReSubscriberArgs e)
        {
            if (e.ReSubscriber.SubscriptionPlan == SubscriptionPlan.Prime)
            {
                //client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
            }
            else
            {
                //client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
            }

            //e.ReSubscriber
            SubHype();
        }

        private void onGiftedSubscription(object sender, OnGiftedSubscriptionArgs e)
        {
            //e.GiftedSubscription.
            SubHype();
        }

        private void SubHype()
        {
            Console.WriteLine("SubHype");
        }

        private void onMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var builder = new StringBuilder();
            builder.Append(e.ChatMessage.Username);
            builder.Append(": ");
            builder.Append(e.ChatMessage.Message);

            Console.WriteLine(builder.ToString());


            int i = 0;

            //foreach (var emote in emoteNames)
            //{
            //    i += Regex.Matches(e.ChatMessage.Message, emote).Count;
            //}
            //
            //if (0 != i)
            //{
            //    Console.WriteLine(i);
            //}

            //if ()
            //{
            //    Console.WriteLine(e.ChatMessage.Message);
            //    //client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(30), "Bad word! 30 minute timeout!");
            //}
        }

        private void onWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (e.WhisperMessage.Username == "my_friend")
            {
                //client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");
            }
        }
    }
}
