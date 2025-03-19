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
        
        // non-stackable, axe
        yield return MoveItem(0,10);
        CheckIfItemMoved(0,10);
        
        // stackable, gold
        yield return MoveItem(3,11);
        CheckIfItemMoved(3,11, 50);

        // non-stackable, axe
        yield return DeleteItem(1);
        CheckIfDeleted(1);

        // stackable, gold
        yield return DeleteItem(11);
        CheckIfDeleted(11);

    }

    private StringParam GetStringParamByIndex(int index)
    {
        if (index >= 0 && index <= 9) return Screens.Inventory.Cell.Pockets;
        if (index >= 10 && index <= 24) return Screens.Inventory.Cell.Backpack;
        throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 23.");
    }

    private IEnumerator OpenInventory()
    {
        yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
    }
    private IEnumerator MoveItem(int fromIndex, int toIndex)
    {
        yield return Commands.DragAndDropCommand(GetStringParamByIndex(fromIndex), fromIndex, GetStringParamByIndex(toIndex), toIndex, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
    }

    private IEnumerator DeleteItem(int index)
    {
        yield return Commands.ClickCellCommand(GetStringParamByIndex(index), index, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Delete, new ResultData<SimpleCommandResult>());
    }


    private void CheckIfDeleted(int index)
    {
        var nonStackableStartIconEmptyChecker = new IconEmptyChecker(Context, GetStringParamByIndex(index), index);
        if (!nonStackableStartIconEmptyChecker.Check()) {
            Fail($"Cell number {index} expected to be empty after deleting");
        }
    }

    private void CheckIfItemMoved(int fromIndex, int toIndex, int? expectedCount = null)
    {
        var nonStackableStartIconEmptyChecker = new IconEmptyChecker(Context, GetStringParamByIndex(fromIndex), fromIndex);
        if (!nonStackableStartIconEmptyChecker.Check()) {
            Fail($"Cell number {fromIndex} expected to be empty after moving");
        }
        var nonStackableEndIconEmptyChecker = new IconEmptyChecker(Context, GetStringParamByIndex(toIndex), toIndex);
        if (nonStackableEndIconEmptyChecker.Check()) {
            Fail($"Cell number {toIndex} expected to be filled after moving");
        }
        if (expectedCount != null){
            var stackableEndCellCountChecker = new CellCountChecker(Context, GetStringParamByIndex(toIndex), toIndex, (int)expectedCount);
            if (!stackableEndCellCountChecker.Check()) {
                Fail($"Cell number {toIndex} wrong count after moving");
            }
        }

    }

}