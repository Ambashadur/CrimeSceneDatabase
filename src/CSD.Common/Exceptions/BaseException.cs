﻿using System;
using System.Net;

namespace CSD.Common.Exceptions;

public abstract class BaseException : Exception
{
    public abstract HttpStatusCode StatusCode { get; }

    public BaseException(string message) : base(message) {

    }

}
