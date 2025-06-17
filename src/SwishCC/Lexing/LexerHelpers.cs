// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace SwishCC.Lexing
{
    public static class LexerHelpers
    {
        public static IEnumerable<char> ExpandRange(this string range)
        {
            var savedChars = new List<char>();

            foreach (var ch in range)
            {
                // If this is a hyphen, there are two cases:
                // 1. The next character will be the end of a range, or
                // 2. This is the last character, and we will want to include a literal hyphen in the set
                if (ch == '-')
                {
                    savedChars.Add(ch);
                    continue;
                }

                // Not a hyphen. Check to see what characters, if any, have been saved.
                if (savedChars.Count == 0)
                {
                    // Nothing has yet been saved. Just add this character, so we can deal with it later.
                    savedChars.Add(ch);
                }
                else if (savedChars.Count == 1)
                {
                    // We have one saved character. Emit it, and save this one.
                    yield return savedChars[0];
                    savedChars[0] = ch;
                }
                else if (savedChars.Count == 2)
                {
                    // This is the last character in a range. Emit the range, and clear the saved characters.
                    for (var j = savedChars[0]; j <= ch; j++)
                    {
                        yield return j;
                    }

                    savedChars.Clear();
                }
            }

            // If there are any saved characters, emit them as literals.
            foreach (var ch in savedChars)
            {
                yield return ch;
            }
        }
    }
}
