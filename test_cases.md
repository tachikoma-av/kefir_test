## 一 У игрока есть инвентарь, предметы из него можно использовать и удалять. 
// TestCase2
open
stackable -> move
check if moved, no duplicate
non-stackable -> move
check if moved, no duplicate

delete stackable
check if empty
delete non stackable
check if empty



## 一 Деревья можно рубить с помощью топора. 
// TestCase3
check if can cut tree
check if axe is getting destroyed after 10 uses
check if can cut without axe

## 一 На станке есть возможность перерабатывать деревья в доски и ускорять этот процесс за монеты.
// TestCase4
Context.Cheats.GetWood(1);
open workbench
place woods into bench
wait X secs
check if null
extract both

Context.Cheats.GetWood(1);
place woods into bench
use coin
extract both
check if null
check if coins deducted

