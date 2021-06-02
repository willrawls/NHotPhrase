/*
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using NHotPhrase.Phrase;

namespace NHotPhrase.Wpf
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design",
        "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "This is a singleton; disposing it would break it")]
    public class HotPhraseManagerForWpf : HotPhraseManager
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        public readonly HwndSource _source;

        public static bool ExecuteCommand(InputBinding binding)
        {
            var command = binding.Command;
            var parameter = binding.CommandParameter;
            var target = binding.CommandTarget;

            if (command == null)
                return false;

            var routedCommand = command as RoutedCommand;
            if (routedCommand != null)
            {
                if (routedCommand.CanExecute(parameter, target))
                {
                    routedCommand.Execute(parameter, target);
                    return true;
                }
            }
            else
            {
                if (command.CanExecute(parameter))
                {
                    command.Execute(parameter);
                    return true;
                }
            }
            return false;
        }
    }
}
*/
