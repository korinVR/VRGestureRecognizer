using UnityEngine;
using System;
using System.Collections.Generic;

namespace ScriptExample
{
    public class SimpleScriptEngine : MonoBehaviour
    {
        public bool IsWaitingYesNo => state == State.WaitingYesNo;

        public event Action<string> MessageUpdated;

        int cursor;
        string[] lines;

        enum State
        {
            Normal,
            WaitingYesNo,
        }

        State state;

        string yesLabel;
        string noLabel;

        const string GestureCommand = "gesture";
        const string DelayCommand = "delay";
        const string GoToCommand = "goto";

        readonly Dictionary<string, int> labels = new Dictionary<string, int>();

        void Start()
        {
            var textAsset = Resources.Load("script") as TextAsset;
            var scriptLines = textAsset.text.Split('\n');

            var dstLines = new List<string>();

            foreach (var scriptLine in scriptLines)
            {
                var line = scriptLine.Trim();

                if (line.Length == 0) continue;

                // Skip comment
                if (line.StartsWith(";")) continue;

                // Add label
                if (line.StartsWith("*"))
                {
                    labels.Add(ParseLabel(line), dstLines.Count);
                    continue;
                }

                dstLines.Add(scriptLine);
            }
            lines = dstLines.ToArray();

            state = State.Normal;
            ExecuteNextCommand();
        }

        static bool TryParseCommand(string line, out string command, out string option)
        {
            var potentialCommandLength = line.IndexOf(' ');

            if (potentialCommandLength > 0)
            {
                var potentialCommand = line.Substring(0, potentialCommandLength);

                if (potentialCommand == GestureCommand ||
                    potentialCommand == DelayCommand ||
                    potentialCommand == GoToCommand)
                {
                    command = potentialCommand;
                    option = line.Substring(potentialCommandLength + 1);
                    return true;
                }
            }

            command = "";
            option = "";
            return false;
        }

        public void ExecuteNextCommand()
        {
            if (TryParseCommand(lines[cursor], out var command, out var option))
            {
                switch (command)
                {
                    case GestureCommand:
                        var yesNoLabels = option.Split(',');
                        if (yesNoLabels.Length != 2) throw new ArgumentException();

                        yesLabel = ParseLabel(yesNoLabels[0]);
                        noLabel = ParseLabel(yesNoLabels[1]);
                        state = State.WaitingYesNo;
                        return;

                    case DelayCommand:
                        if (!int.TryParse(option, out var time)) throw new ArgumentException();

                        cursor++;
                        Invoke(nameof(ExecuteNextCommand), time / 1000f);
                        return;

                    case GoToCommand:
                        GoTo(ParseLabel(option));
                        return;
                }
            }

            var message = "";
            while (!TryParseCommand(lines[cursor], out _, out _))
            {
                message += lines[cursor++] + "\n";
            }
            
            MessageUpdated?.Invoke(message);
        }

        public void GoTo(string label)
        {
            cursor = labels[label];
            ExecuteNextCommand();
        }

        public void AnswerYes()
        {
            if (state != State.WaitingYesNo) return;
            
            state = State.Normal;
            GoTo(yesLabel);
        }

        public void AnswerNo()
        {
            if (state != State.WaitingYesNo) return;
            
            state = State.Normal;
            GoTo(noLabel);
        }

        static string ParseLabel(string text)
        {
            text = text.Trim();
            
            if (text.StartsWith("*"))
            {
                return text.Substring(1);
            }

            throw new ArgumentException();
        }
    }
}
