namespace YerdenYuksek.Application.Services.Public.Messages;

public interface ITokenizer
{
    string Replace(string template, IEnumerable<Token> tokens, bool htmlEncode);
}
