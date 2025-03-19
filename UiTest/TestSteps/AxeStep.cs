using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.UiTest.Context.Consts;
using Assets.UiTest.Results;
using UiTest.UiTest.Checker;

namespace Assets.UiTest.TestSteps;

public class AxeStep : UiTestStepBase
{
    public override string Id => "axe_step";
    public override double TimeOut => 300;

    protected override Dictionary<string, string> GetArgs()
    {
        return new Dictionary<string, string>();
    }

    protected override IEnumerator OnRun()
    {
        yield return OpenInventory();
        yield return DeleteAxes();
        yield return CloseInventory();
        yield return GoToFirstTree();
        yield return CutTree();
        yield return CheckWoodCountAtInventory();
    }

    private IEnumerator OpenInventory()
    {
        yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
    }

    private IEnumerator DeleteAxes()
    {
        for (int i = 0; i < 3; i++)
        {
            var startItemLoc = Screens.Inventory.Cell.Pockets;
            yield return Commands.ClickCellCommand(startItemLoc, i, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Delete, new ResultData<SimpleCommandResult>());
        
        }
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());

    }
    private IEnumerator GoToFirstTree()
    {
        var trees = Cheats.FindTree();
        yield return Commands.PlayerMoveCommand(trees[0].transform.position, new ResultData<PlayerMoveResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
    }
    private IEnumerator CutTree()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return Commands.UseButtonClickCommand(Screens.Main.Button.Use, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        }
    }
    private IEnumerator CheckWoodCountAtInventory()
    {
        yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());

        var cellIsEmptyChecker = new CountIsEmptyChecker(Context, Screens.Inventory.Cell.Pockets, 0);
        if (!cellIsEmptyChecker.Check())
        {
            Fail("Wrong wood count at pockets cell with index 0");
        }

        yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Close, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
    }

    private IEnumerator CloseInventory()
    {
        yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Close, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
    }
}