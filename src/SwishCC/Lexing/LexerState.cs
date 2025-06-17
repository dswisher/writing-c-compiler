// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;

namespace SwishCC.Lexing
{
    public class LexerState
    {
        private readonly Dictionary<int, Action<int, LexerContext>> transitions = new();
        private Action<int, LexerContext> defaultAction;


        public LexerState(string name)
        {
            Name = name;
        }


        public string Name { get; }


        public LexerState AddTransition(int ch, Action<int, LexerContext> action)
        {
            if (!transitions.TryAdd(ch, action))
            {
                throw new LexException($"Transition for character '{ch}' already exists in state {Name}");
            }

            return this;
        }


        public LexerState AddTransition(string range, Action<int, LexerContext> action)
        {
            foreach (var ch in range.ExpandRange())
            {
                if (!transitions.TryAdd(ch, action))
                {
                    throw new LexException($"Transition for character '{ch}' already exists in state {Name}");
                }
            }

            return this;
        }


        public LexerState Otherwise(Action<int, LexerContext> action)
        {
            if (defaultAction != null)
            {
                throw new LexException($"Default action already set for state {Name}");
            }

            defaultAction = action;

            return this;
        }


        public void Execute(LexerContext context)
        {
            // Peek at the next character
            var ch = context.LexerReader.Peek();

            // Find the transition and execute the action
            if (transitions.TryGetValue(ch, out var action))
            {
                // Execute the action for the transition
                action(ch, context);
            }
            else if (defaultAction != null)
            {
                defaultAction(ch, context);
            }
            else
            {
                throw new LexException($"Unexpected character '{(char)ch}' (0x{ch:X2}) in state {Name}");
            }
        }
    }
}
