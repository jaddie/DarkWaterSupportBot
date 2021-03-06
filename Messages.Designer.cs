﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]

namespace DarkwaterSupportBot
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class MessagesContainer : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new MessagesContainer object using the connection string found in the 'MessagesContainer' section of the application configuration file.
        /// </summary>
        public MessagesContainer() : base("name=MessagesContainer", "MessagesContainer")
        {
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new MessagesContainer object.
        /// </summary>
        public MessagesContainer(string connectionString) : base(connectionString, "MessagesContainer")
        {
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new MessagesContainer object.
        /// </summary>
        public MessagesContainer(EntityConnection connection) : base(connection, "MessagesContainer")
        {
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Message> Messages
        {
            get
            {
                if ((_Messages == null))
                {
                    _Messages = base.CreateObjectSet<Message>("Messages");
                }
                return _Messages;
            }
        }
        private ObjectSet<Message> _Messages;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Messages EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToMessages(Message message)
        {
            base.AddObject("Messages", message);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Messages", Name="Message")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Message : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Message object.
        /// </summary>
        /// <param name="dateLeft">Initial value of the DateLeft property.</param>
        /// <param name="ircNick">Initial value of the IrcNick property.</param>
        /// <param name="messageText">Initial value of the MessageText property.</param>
        /// <param name="fromIrcNick">Initial value of the FromIrcNick property.</param>
        public static Message CreateMessage(global::System.String dateLeft, global::System.String ircNick, global::System.String messageText, global::System.String fromIrcNick)
        {
            Message message = new Message();
            message.DateLeft = dateLeft;
            message.IrcNick = ircNick;
            message.MessageText = messageText;
            message.FromIrcNick = fromIrcNick;
            return message;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String DateLeft
        {
            get
            {
                return _DateLeft;
            }
            set
            {
                if (_DateLeft != value)
                {
                    OnDateLeftChanging(value);
                    ReportPropertyChanging("DateLeft");
                    _DateLeft = StructuralObject.SetValidValue(value, false);
                    ReportPropertyChanged("DateLeft");
                    OnDateLeftChanged();
                }
            }
        }
        private global::System.String _DateLeft;
        partial void OnDateLeftChanging(global::System.String value);
        partial void OnDateLeftChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String IrcNick
        {
            get
            {
                return _IrcNick;
            }
            set
            {
                OnIrcNickChanging(value);
                ReportPropertyChanging("IrcNick");
                _IrcNick = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("IrcNick");
                OnIrcNickChanged();
            }
        }
        private global::System.String _IrcNick;
        partial void OnIrcNickChanging(global::System.String value);
        partial void OnIrcNickChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String MessageText
        {
            get
            {
                return _MessageText;
            }
            set
            {
                OnMessageTextChanging(value);
                ReportPropertyChanging("MessageText");
                _MessageText = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("MessageText");
                OnMessageTextChanged();
            }
        }
        private global::System.String _MessageText;
        partial void OnMessageTextChanging(global::System.String value);
        partial void OnMessageTextChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String FromIrcNick
        {
            get
            {
                return _FromIrcNick;
            }
            set
            {
                OnFromIrcNickChanging(value);
                ReportPropertyChanging("FromIrcNick");
                _FromIrcNick = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("FromIrcNick");
                OnFromIrcNickChanged();
            }
        }
        private global::System.String _FromIrcNick;
        partial void OnFromIrcNickChanging(global::System.String value);
        partial void OnFromIrcNickChanged();

        #endregion
    
    }

    #endregion
    
}
