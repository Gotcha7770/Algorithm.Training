using System;
using System.Buffers.Text;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using FluentAssertions;
using Xunit;

namespace Algorithm.Training;

// https://confluence.atlassian.com/confkb/how-to-programmatically-generate-the-tiny-link-of-a-confluence-page-956713432.html
public class Task9
{
    private readonly LinkFactory _factory = new("http://confluence.test.ru/");

    [Theory]
    [InlineData(3917429, "dcY7")]
    [InlineData(3900673, "AYU7")]
    [InlineData(43900673, "Ad_dAg")]
    public void TinyId(long input, string output)
    {
        _factory.ToShortId(input)
            .Should()
            .Be(output);
    }

    [Theory]
    [InlineData("http://confluence.test.ru/pages/viewpage.action?pageId=3917429", "http://confluence.test.ru/x/dcY7")]
    public void TinyLink(string input, string output)
    {
        _factory.Create(input)
            .Should()
            .Be(output);
    }

    [Theory]
    [InlineData(3917429, "dcY7")]
    [InlineData(3900673, "AYU7")]
    [InlineData(43900673, "Ad+dAg")]
    public void Base64Class(long input, string output)
    {
        byte[] bytes = BitConverter.GetBytes(input);
        int firstZeroIndex = Array.IndexOf(bytes, (byte)0);
        Span<byte> utf8 = stackalloc byte[bytes.Length];
        var result = Base64.EncodeToUtf8(
            bytes.AsSpan(0, firstZeroIndex),
            utf8,
            out int consumed,
            out int written);

        Span<char> chars = stackalloc char[written];
        Encoding.UTF8.GetChars(utf8[..written], chars);
        Encoding.UTF8.GetString(utf8[..written])
            .Should()
            .Be(output);
    }
}

public class LinkFactory
{
    private readonly string _baseUrl;

    public LinkFactory(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public string Create(string input)
    {
        var uri = new Uri(input);
        long pageId = HttpUtility.ParseQueryString(uri.Query)
            .GetValue<long>("pageId");
        
        var tinyId = ToShortId(pageId);
        
        return $"{_baseUrl}x/{tinyId}";
    }

    public string ToShortId(long id)
    {
        byte[] bytes = BitConverter.GetBytes(id);
        int firstZeroIndex = Array.IndexOf(bytes, (byte)0);
        string base64 = Convert.ToBase64String(bytes[..firstZeroIndex]);
        var sequence = base64
            .Replace('/', '-')
            .Replace('+', '_')
            .TakeWhile(x => x != '=');

        return new string(sequence.ToArray());
    }
}

public static class Extensions
{
    public static T GetValue<T>(this NameValueCollection source, string key)
    {
        string value = source.Get(key);
        var converter = TypeDescriptor.GetConverter(typeof(T));

        return converter.CanConvertFrom(typeof(string)) 
            ? (T)converter.ConvertFrom(value) 
            : default;
    }
}