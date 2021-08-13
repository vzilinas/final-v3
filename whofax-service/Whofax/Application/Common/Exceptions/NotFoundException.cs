﻿using Whofax.Application.Resources;

namespace Whofax.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
        : base()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public NotFoundException(string name, object key)
        : base(string.Format(ErrorMessages.EntityNotFound, name, key))
    {
    }
}