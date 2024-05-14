using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RoomCreationException : Exception
{
    public string Title { get; private set; }  // Title 속성 추가

    public RoomCreationException(string title, string message) : base(message)
    {
        Title = title;
    }

}
