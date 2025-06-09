INCLUDE globals.ink

Доров! #speaker:Doctor #portrait:doctor #layout:left #fade:in
-> main

===main===

Як почуваєш <b>себе</b> сьогодні? 
+ [Добре]
    Тоді і мені добре! #portrait:doctor_happy
+ [Погано]
    О, мені тоді теж сумно! #portrait:doctor_sad
    
- Не довіряй йому, він брешить! #speaker:Villager #portrait:Villager #layout:right

Маєш ще запитання? #speaker:Doctor #portrait:doctor #layout:left
+[Так]
 -> main
+[Ні]
    Тоді до зустрічі!
    :#picture:test #layout:hide 
    :#fade:out
    ->END