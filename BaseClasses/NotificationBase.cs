using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BaseClasses.Annotations;

namespace BaseClasses
{
    public class NotificationBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    //public enum State
    //{
    //    Idle = 0,
    //    InProgress,
    //    Error
    //}


    public sealed class State
    {
        public static readonly State Ready = new State("Ready", "The software is ready for working.", 3);
        public static readonly State InProgress = new State("InProgress", "There is an operation in the background.", 2);
        public static readonly State Idle = new State("Idle", "The software is un-initialized.", 1);
        public static readonly State Error = new State("Error", "The software cannot operate, restart is proposed.", 0);


        public string Name { get; }
        public string Description { get; }

        public int Value { get; }


        public State(string name, string description, int value)
        {
            Name = name;
            Description = description;
            Value = value;
        }


        public static State operator &(State left, State right)
        {
            int lowerValue = left.Value <= right.Value ? left.Value : right.Value;

            switch (lowerValue)
            {
                case 0:
                    return new State("Error", "The software cannot operate, restart is proposed.", 0);
                case 1:
                    return new State("Idle", "The software is un-initialized.", 1);
                case 2:
                    return new State("InProgress", "There is an operation in the background.", 2);
                default:
                    return new State("Ready", "The software is ready for working.", 3);
            }
        }

    }

}
