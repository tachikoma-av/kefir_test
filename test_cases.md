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
cut 1 tree
goto workbench
open workbench
check if have woods
3 woods
place woods into bench
wait X secs
extract both
2 woods + 1 refined 

place woods into bench
use coin
extract both
1 woods + 2 refined
