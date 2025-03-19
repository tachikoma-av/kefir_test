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
        yield return GoToFirstTree();
        yield return CutTree();
        yield return CheckWoodCountAtInventory(4,3, "Wrong wood count after cutting 1 tree");

        yield return BreakAxe();
        yield return CheckIfAxeGotBroken();

        yield return DeleteOtherAxes();
        yield return CutTree(2);
        yield return CheckWoodCountAtInventory(0,0, "Was able to complete cutting tree without axe in 2 cuts");
    }

    private IEnumerator OpenInventory()
    {
        yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
    }
    private IEnumerator CloseInventory()
    {
        yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Close, new ResultData<SimpleCommandResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
    }

    private IEnumerator CheckWoodCountAtInventory(int cell, int count, string failReason)
    {
        yield return OpenInventory();
        var cellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, cell, count);
        if (!cellCountChecker.Check())
        {
            Fail(failReason);
        }
        yield return CloseInventory();

    }

    private IEnumerator DeleteOtherAxes()
    {
        yield return OpenInventory();
        for (int i = 1; i < 3; i++)
        {
            var startItemLoc = Screens.Inventory.Cell.Pockets;
            yield return Commands.ClickCellCommand(startItemLoc, i, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Delete, new ResultData<SimpleCommandResult>());
        
        }
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        yield return CloseInventory();
    }
    private IEnumerator GoToFirstTree()
    {
        var trees = Cheats.FindTree();
        yield return Commands.PlayerMoveCommand(trees[0].transform.position, new ResultData<PlayerMoveResult>());
        yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
    }
    private IEnumerator BreakAxe()
    {
        var trees = Cheats.FindTree();
        for (int i = 1; i < 4; i++)
        {
            yield return Commands.PlayerMoveCommand(trees[i].transform.position, new ResultData<PlayerMoveResult>());
            if (i == 3){
                yield return CutTree(1);
            } else {
                yield return CutTree();
            }
        }
    }
    private IEnumerator CheckIfAxeGotBroken()
    {
        yield return OpenInventory();
        var nonStackableStartIconEmptyChecker = new IconEmptyChecker(Context, Screens.Inventory.Cell.Pockets, 0);
        if (!nonStackableStartIconEmptyChecker.Check()) {
            Fail($"Cell number {0} expected to be empty after breaking axe");
        }
        yield return CloseInventory();
    }



    private IEnumerator CutTree(int times = 3)
    {
        for (int i = 0; i < times; i++)
        {
            yield return Commands.UseButtonClickCommand(Screens.Main.Button.Use, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        }
    }


}