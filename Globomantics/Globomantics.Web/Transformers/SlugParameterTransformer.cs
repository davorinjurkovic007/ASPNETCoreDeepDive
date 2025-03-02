using System.Text.RegularExpressions;

namespace Globomantics.Web.Transformers;

/// <summary>
/// I'd like to ensure that even if it's a match or not, it will tranform my parameters into something
/// that I've predefined
/// Ideally, I'd like to define in my route how to transform the slug into the appropriate string.
/// To do this, we can introduce an IOutboundParameterTransformer
/// Slug: https://developer.mozilla.org/en-US/docs/Glossary/Slug
/// A Slug is the unique identifying part of a web address, typically at the end of the URL. 
/// In the context of MDN, it is the portion of the URL following "<locale>/docs/".
/// 
/// It may also just be the final component when a new document is created under a parent document;
/// for example, this page's slug is Glossary/Slug.
/// 
/// TransformOutbound means that when this is going to be the otugoing value, it will run this tranfsormer. 
/// 
/// I'm going to take the value passed into this tranformer. 
/// I'm going to ensure thta we replaced everything but the alfphanumeric to a dash.
/// We're going to use culture invariants.
/// And then we're going to set this to also have a time limit
/// The last portion for Regex.Replace is for how long we're allowed to run this. 
/// It's wise to have a pretty short timeout. 
/// Otherwise, you might run into some annoying attackas against your system where it heavily runs CPU operations.
/// </summary>
public class SlugParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if(value is not string)
        {
            return null;
        }

        return Regex.Replace(value.ToString()!, @"[^a-zA-Z0-9]+", "-",
                       RegexOptions.CultureInvariant,
                       TimeSpan.FromMicroseconds(2000)).ToLowerInvariant().Trim('-');
    }
}
