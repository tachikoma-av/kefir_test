using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.UiTest.Context.Consts;
using Assets.UiTest.Results;
using UiTest.UiTest.Checker;

namespace Assets.UiTest.TestSteps;

public class InventoryStep : UiTestStepBase
{
    public override string Id => "inventory_step";
    public override double TimeOut => 300;

    protected override Dictionary<string, string> GetArgs()
    {
        return new Dictionary<string, string>();
    }

    protected override IEnumerator OnRun()
    {
        yield return OpenInventory();
        yield return CheckMoveItem();
        yield return CheckDeleteItem();
        yield return CloseInventory();
    }

    private IEnumerator OpenInventory()
    {
        yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
    }

    // private StringParam GetStorageTypeByIndex(int index){
    //     // 0-9 cells - Pockets, Cell 10-23 - Backpack 
    // }

    private IEnumerator CheckMoveItem()
    {
        var startItemLoc = Screens.Inventory.Cell.Pockets;
        var startItemIndex = 0;
        
        var endItemLoc = Screens.Inventory.Cell.Backpack;
        var endItemIndex = 10;
        
        // 0-9 cells - Pockets, Cell 10-23 - Backpack 
        yield return Commands.DragAndDropCommand(startItemLoc, startItemIndex, endItemLoc, endItemIndex, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());

        var startCellIsEmptyChecker = new CountIsEmptyChecker(Context, startItemLoc, startItemIndex);
        var cell_status = startCellIsEmptyChecker.Check(); 
        if (startCellIsEmptyChecker.Check())
        {
            Fail($"cell number {startItemIndex} expected to be empty after moving {cell_status}");
        }
        var endCellIsEmptyChecker = new CountIsEmptyChecker(Context, endItemLoc, endItemIndex);
        if (!endCellIsEmptyChecker.Check())
        {
            Fail($"cell number ${endItemIndex} expected to be filled after moving");
        }
    }
    private IEnumerator CheckDeleteItem()
    {
        var startItemLoc = Screens.Inventory.Cell.Pockets;
        var startItemIndex = 1;
        // 0-9 cells - Pockets, Cell 10-24 - Backpack 
        yield return Commands.ClickCellCommand(startItemLoc, startItemIndex, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Delete, new ResultData<SimpleCommandResult>());
        var startCellIsEmptyChecker = new CountIsEmptyChecker(Context, startItemLoc, startItemIndex);
        if (!startCellIsEmptyChecker.Check())
        {
            Fail($"cell number ${startItemIndex} expected to be empty after deleting");
        }
    }


    private IEnumerator CloseInventory()
    {
        yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Close, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
    }
}