INCLUDE ../globals.ink
EXTERNAL DoorActivation()

-> main

=== main ===
{wasBookFound == false  && wasRulerFound == false && wasTestFound == false:
Ти ще не можеш піти. #layout:right #fade:in
#fade:out
->END
}

Як пройшов день? #layout:right #fade:in
{wasTestFound:
    *[Записка]
        -> note
}
{wasBookFound:
    *[Книга]
        -> book
}
{wasRulerFound:
    *[Прикраса]
        -> jewelry
}
{wasBookFound == false  || wasRulerFound == false || wasTestFound == false:
    *[Назад]
    #fade:out
        ->END
}


=== note ===
Це… буває трохи лякаюче. Бути з кимось. #layout:right #speaker:???
Небагато… начебто ти не в собі. Відпускаєш частину себе гуляти окремо.
Береш обіцянку, що ця частина тебе, незважаючи на всі негаразди, завжди повертатиметься.
Зрештою… Від страху втрачаєш те цінне, що намагаєшся вберегти.
Тільки щоб не втратити, як тобі здається, це згодом під час нової «катастрофи».

#fade:out
~DoorActivation() 

-> END

=== book ===
Іноді здається, що це <i>«щось»</i> найбільше, що є між вами. #layout:right #speaker:???
І за цим завжди буде провал, начебто не впораєтеся.
Чергова така <i>«катастрофа»</i> завжди здається критичною. Останньою краплею.
… #layout:leftPortrait #portrait:mc_idle #speaker:
Найчастіше це не так. Тобі не потрібно бути кимось іншим, щоб уникати того, що трапилося.#layout:right #speaker:???
Тільки пізно усвідомлюєш.

:#layout:hideRight #picture:Silhouette_behind 

#fade:out
~DoorActivation() 

-> END

=== jewelry ===
Речі зберігають у собі пам'ять. #layout:right #speaker:???
Вона не завжди про те, що є швидше про те, що залишилося.
Все залишається, щоправда, не так, як раніше.
Змінюється.

:#layout:hideRight #picture:Silhouette_disappearing 

#fade:out
~DoorActivation() 

-> END