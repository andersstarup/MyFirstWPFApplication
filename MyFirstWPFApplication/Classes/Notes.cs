using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstWPFApplication.Classes
{

}

/* foreach (var Root.Params in myDeserializedClass)
 {
     Scroller.Content += "hej"  + Environment.NewLine;
 }*/
/*
foreach (var root in e.AddedItems.Cast<Board>())
{
    foreach (var command in board.commands)
    {
        Button button = new Button() { Content = command }; // Creating button
        button.Name = command + "_btn";

        button.Click += new RoutedEventHandler(button_click);
        this.FuncSelect.Children.Add(button);
    }
}
*/
/*
if (array[0] == '0')
{
    Waiting.Visibility = Visibility.Collapsed;
    boardNr ++;
    Board board = new Board();
    board.B_ID = boardNr;

    Scroller.Content += "Found a init message at: " + board.B_Name + Environment.NewLine;

    if (array[1] == '1')
    {
        Scroller.Content += "This Board has a LED" + Environment.NewLine;
        board.commands.Add("Led");
    }

    if (array[2] == '1')
    {
        Scroller.Content += "This Board has Stepper motor" + Environment.NewLine;
        board.commands.Add("StepM");
    }

    if (array[3] == '1')
    {
        Scroller.Content += "This Board has Fork sensor" + Environment.NewLine;
        board.commands.Add("Fork");
    }

    boards.Add(board);                  // Tilføj board til listen boards.
    boxBoards.ItemsSource = boards;     // Tilføj all boards til comboboxen i WPF.
}
*/

/*
else if (array[0] == '1')
{
    Scroller.Content += "Not a init message" + Environment.NewLine;
    array[0] = 140;
} 
*/
