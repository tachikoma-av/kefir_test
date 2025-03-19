using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.UiTest.Context.Consts;
using Assets.UiTest.Results;
using UiTest.UiTest.Checker;

namespace Assets.UiTest.TestSteps;

public class WorkbenchStep : UiTestStepBase
{
    public override string Id => "workbench_step";
    public override double TimeOut => 300;

    protected override Dictionary<string, string> GetArgs()
    {
        return new Dictionary<string, string>();
    }

    protected override IEnumerator OnRun()
    {
        Context.Cheats.GetWood(1);
        yield return OpenWorkbench();
        
        // place woods to workbench
        yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.Pockets, 4, Screens.Inventory.Cell.WorkbenchRow, 0, new ResultData<SimpleCommandResult>());
        // wait till melted
        yield return Commands.WaitForSecondsCommand(6, new ResultData<SimpleCommandResult>());

        if (!new IconEmptyChecker(Context, Screens.Inventory.Cell.WorkbenchRow, 0).Check()) Fail($"WorkbenchRow must be empty after recycling");
        if (new IconEmptyChecker(Context, Screens.Inventory.Cell.WorkbenchResult, 0).Check()) Fail($"WorkbenchResult must containt smth after recycling");

        // extract result
        yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.WorkbenchResult, 0, Screens.Inventory.Cell.Pockets, 4, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        
        if (!new IconEmptyChecker(Context, Screens.Inventory.Cell.WorkbenchResult, 0).Check()) Fail($"WorkbenchResult must be empty after withdrawing");

        Context.Cheats.GetWood(1);
        // place another one
        yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.Pockets, 5, Screens.Inventory.Cell.WorkbenchRow, 0, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        // use booster
        yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Skip, new ResultData<SimpleCommandResult>());

        if (!new IconEmptyChecker(Context, Screens.Inventory.Cell.WorkbenchRow, 0).Check()) Fail($"WorkbenchRow must be empty after recycling with skip");
        if (new IconEmptyChecker(Context, Screens.Inventory.Cell.WorkbenchResult, 0).Check()) Fail($"WorkbenchResult must containt smth after recycling with skip");

        // extract result
        yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.WorkbenchResult, 0, Screens.Inventory.Cell.Pockets, 4, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());

        // check if coins were deducted
        if (!new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, 3, 45).Check()) Fail($"5 coins must be deducted");
    }
    private IEnumerator OpenWorkbench()
    {
        yield return Commands.FindAndGoToSingleObjectCommand(Locations.Home.WorkbenchSawmill, new ResultData<PlayerMoveResult>());
        yield return Commands.UseButtonClickCommand(Screens.Main.Button.Use, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
    }
}