﻿using System.Linq;
using SteamKit2;
using SteamTrade;

namespace SteamBot
{
    /// <summary>
    /// The abstract base class for users of SteamBot that will allow a user
    /// to extend the functionality of the Bot.
    /// </summary>
    public abstract class UserHandler
    {
        protected Bot Bot;
        protected SteamID OtherSID;
        protected SteamID MySID;

        public UserHandler(Bot bot, SteamID sid)
        {
            Bot = bot;
            OtherSID = sid;
            MySID = bot.SteamUser.SteamID;
        }

        /// <summary>
        /// Gets the Bot's current trade.
        /// </summary>
        /// <value>
        /// The current trade.
        /// </value>
        public Trade Trade
        {
            get
            {
                return Bot.CurrentTrade;
            }
        }

        /// <summary>
        /// Gets the log the bot uses for convenience.
        /// </summary>
        protected Log Log
        {
            get { return Bot.log; }
        }

        /// <summary>
        /// Gets a value indicating whether the other user is admin.
        /// </summary>
        /// <value>
        /// <c>true</c> if the other user is a configured admin; otherwise, <c>false</c>.
        /// </value>
        protected bool IsAdmin
        {
            get { return Bot.Admins.Contains(OtherSID); }
        }

        /// <summary>
        /// Called when the bot is invited to a Steam group
        /// </summary>
        /// <returns>
        /// Whether to accept.
        /// </returns>
        public abstract bool OnGroupAdd();

        /// <summary>
        /// Called when the user adds the bot as a friend.
        /// </summary>
        /// <returns>
        /// Whether to accept.
        /// </returns>
        public abstract bool OnFriendAdd();

        /// <summary>
        /// Called when the user removes the bot as a friend.
        /// </summary>
        public abstract void OnFriendRemove();

        /// <summary>
        /// Called whenever a message is sent to the bot.
        /// This is limited to regular and emote messages.
        /// </summary>
        public abstract void OnMessage(string message, EChatEntryType type);

        /// <summary>
        /// Called when the bot is fully logged in.
        /// </summary>
        public abstract void OnLoginCompleted();

        /// <summary>
        /// Called whenever a user requests a trade.
        /// </summary>
        /// <returns>
        /// Whether to accept the request.
        /// </returns>
        public abstract bool OnTradeRequest();

        /// <summary>
        /// Called when a chat message is sent in a chatroom
        /// </summary>
        /// <param name="chatID">The SteamID of the group chat</param>
        /// <param name="sender">The SteamID of the sender</param>
        /// <param name="message">The message sent</param>
        public virtual void OnChatRoomMessage(SteamID chatID, SteamID sender, string message)
        {

        }

        /// <summary>
        /// Called when an 'exec' command is given via botmanager.
        /// </summary>
        /// <param name="command">The command message.</param>
        public virtual void OnBotCommand(string command)
        {

        }

        /// <summary>
        /// Called when user accepts or denies bot's trade request.
        /// </summary>
        /// <param name="accepted">True if user accepted bot's request, false if not.</param>
        /// <param name="response">String response of the callback.</param>
        public virtual void OnTradeRequestReply(bool accepted, string response)
        {

        }

        #region Trade events
        // see the various events in SteamTrade.Trade for descriptions of these handlers.

        public abstract void OnTradeError(string error);

        public abstract void OnTradeTimeout();

        public abstract void OnTradeSuccess();

        public virtual void OnTradeClose()
        {
            Bot.log.Warn("[USERHANDLER] TRADE CLOSED");
            Bot.CloseTrade();
        }

        public abstract void OnTradeInit();

        public abstract void OnTradeAddItem(GenericInventory.Inventory.Item inventoryItem);

        public abstract void OnTradeRemoveItem(GenericInventory.Inventory.Item inventoryItem);

        public abstract void OnTradeMessage(string message);

        public void OnTradeReadyHandler(bool ready)
        {
            Trade.Poll();
            OnTradeReady(ready);
        }

        public abstract void OnTradeReady(bool ready);

        public void OnTradeAcceptHandler()
        {
            Trade.Poll();
            if (Trade.OtherIsReady && Trade.MeIsReady)
            {
                OnTradeAccept();
            }
        }

        public abstract void OnTradeAccept();

        public abstract void SetChatStatus(string message);

        public abstract void SetStatus(EPersonaState state);

        public abstract bool OpenChat(SteamID SID);

        public abstract void SendTradeState(uint tradeID);

        public abstract void SendTradeError(string message);

        #endregion Trade events
    }
}
