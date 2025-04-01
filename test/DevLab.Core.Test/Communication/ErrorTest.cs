using DevLab.Core.Communication;
using FluentAssertions;

namespace DevLab.Core.Test.Communication;

public class ErrorTest
{
    [Fact(DisplayName = "Deve retornar o erro None corretamente")]
    [Trait("Category", "Unit")]
    public void Error_None_ShouldReturnCorrectly()
    {
        // Arrange & Act
        var error = Error.None;

        // Assert
        error.Message.Should().Be("None");
        error.Code.Should().Be("No error");
    }

    [Fact(DisplayName = "Deve retornar o erro NullValue corretamente")]
    [Trait("Category", "Unit")]
    public void Error_NullValue_ShouldReturnCorrectly()
    {
        // Arrange & Act
        var error = Error.NullValue;

        // Assert
        error.Message.Should().Be("NullValue");
        error.Code.Should().Be("Value is null");
    }

    [Fact(DisplayName = "Deve retornar a string formatada corretamente quando o código está presente")]
    [Trait("Category", "Unit")]
    public void ToString_WithCode_ShouldReturnFormattedString()
    {
        // Arrange
        var error = new Error("Test message", "Test code");

        // Act
        var result = error.ToString();

        // Assert
        result.Should().Be("Test code: Test message");
    }

    [Fact(DisplayName = "Deve retornar a mensagem quando o código não está presente")]
    [Trait("Category", "Unit")]
    public void ToString_WithoutCode_ShouldReturnMessage()
    {
        // Arrange
        var error = new Error("Test message");

        // Act
        var result = error.ToString();

        // Assert
        result.Should().Be("Test message");
    }
}