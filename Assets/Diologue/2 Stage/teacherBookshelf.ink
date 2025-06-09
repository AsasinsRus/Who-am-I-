INCLUDE ../globals.ink

-> main

=== main ===
{wasBookFound:
    -> second
}
Ви знайшли книгу. #layout:rightPortrait #portrait:book #fade:in
~wasBookFound = true
Виглядає логічно. #layout:leftPortrait #portrait:mc_idle
Ви відкриваєте книгу. #layout:right
На титулу нерозбірливий почерк та підпис.
Ви гортаєте далі і, вдивляючись у рядки:

<i>Стираєш спогади, перетворюючи на секунди миті, Я витрачу все життя на чергову відповідність</i>

Ти байдуже закриваєш книгу, вирішуючи не дочитувати далі.
Ти кладеш книгу на місце.

#fade:out

-> END

=== second ===
Ви вже бачили цю книгу. #layout:rightPortrait #portrait:book #fade:in
#fade:out

->END
