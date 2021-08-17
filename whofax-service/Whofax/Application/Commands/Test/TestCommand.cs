using FluentValidation;
using MediatR;

namespace Whofax.Application.Commands.Test;

public class TestCommand : IRequest
{
    public TestCommand(string testField)
    {
        TestField = testField;
    }

    public string TestField { get; }
}

public class TestCommandValidator : AbstractValidator<TestCommand>
{
    public TestCommandValidator()
    {
        RuleFor(x => x.TestField)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Value cannot be empty")
            .Must(ContainsSomeString).WithMessage("Must contain 'test'")
            .Length(6).WithMessage("Max length exceeded"); ;
    }

    private bool ContainsSomeString(string testField)
    {
        return testField.Contains("test");
    }
}

public class TestCommandHandler : IRequestHandler<TestCommand>
{
    public async Task<Unit> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new Unit());
    }
}
