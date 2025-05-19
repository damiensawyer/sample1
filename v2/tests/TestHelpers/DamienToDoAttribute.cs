// (c) D. Sawyer <damiensawyer@gmail.com> 2025

namespace TestHelpers;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class DamienToDoAttribute(string message) : Attribute
{
  public string Message { get; set; } = message;
}
