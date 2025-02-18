﻿using System.Text.RegularExpressions;

namespace MsbtLib.Controls
{
    internal class ChoiceTwo : Control
    {
        public const ushort tag_group = 0x0001;
        public const ushort tag_type = 0x0004;
        private ushort param_size;
        private ushort choice_0;
        private ushort choice_1;
        private ushort cancel_index;
        public ChoiceTwo(List<ushort> parameters)
        {
            param_size = parameters[0];
            choice_0 = parameters[1];
            choice_1 = parameters[2];
            cancel_index = parameters[3];
        }
        public ChoiceTwo(string str)
        {
            Regex pattern = new(@"<choice2\s0=(\d+)\s1=(\d+)\scancel=(\d+)\s/>");
            Match m = pattern.Match(str);
            if (!m.Success)
            {
                throw new ArgumentException(@"Proper usage: <choice2 0=# 1=# cancel=# /> where all # are 16-bit 
                    integers, and cancel is the 0-based index of the choice that ends the dialogue. Valid examples: 
                    <choice2 0=0 1=1 cancel=1 /> or 
                    <choice2 0=16 1=17 cancel=1 />");
            }
            choice_0 = ushort.Parse(m.Groups[1].ToString());
            choice_1 = ushort.Parse(m.Groups[2].ToString());
            cancel_index = ushort.Parse(m.Groups[3].ToString());
            param_size = 6;
        }
        public override byte[] ToControlSequence(EndiannessConverter converter)
        {
            byte[] bytes = new byte[param_size + 8];
            bytes.Merge(converter.GetBytes(control_tag), 0);
            bytes.Merge(converter.GetBytes(tag_group), 2);
            bytes.Merge(converter.GetBytes(tag_type), 4);
            bytes.Merge(converter.GetBytes(param_size), 6);
            bytes.Merge(converter.GetBytes(choice_0), 8);
            bytes.Merge(converter.GetBytes(choice_1), 10);
            bytes.Merge(converter.GetBytes(cancel_index), 12);
            return bytes;
        }
        public override string ToControlString()
        {
            return $"<choice2 0={choice_0} 1={choice_1} cancel={cancel_index} />";
        }
    }
}
