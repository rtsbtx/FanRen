Talk(0, "请问是乔帮主吗？", "talkname0", 1);
Talk(50, "很好，年纪轻轻，就破了我丐帮的打狗阵，现在的年轻人是越来越厉害了．", "talkname50", 0);
Talk(0, "那里，比起乔帮主，统领这么多英雄好汉，在下还差的远了．", "talkname0", 1);
Talk(50, "一群叫化子，没什么．不知少侠来我丐帮有什么要事吗？", "talkname50", 0);
Talk(0, "没什么特别的．小弟初涉江湖，当然要来丐帮拜访一下，否则怎么称的上是江湖中人．", "talkname0", 1);
Talk(50, "不知江湖行的感觉如何？", "talkname50", 0);
Talk(0, "辛酸，甘甜皆有．有许多事情要去解决，但也学会了许多武功，随着日子一天天过去，觉得自己越发充实．不过江湖上，人心险恶，但我等又脱离不了这江湖，真是所谓”人在江湖，身不由己”．", "talkname0", 1);
Talk(50, "哈！哈！说的好，说的好，别提这些恼人的事了，咱们喝酒．", "talkname50", 0);
if InTeam(35) == false then goto label0 end;
    Talk(35, "是啊，兄弟，我们一起跟乔帮主喝一杯．", "talkname35", 1);
::label0::
    Talk(0, "好．", "talkname0", 1);
    DarkScence();
    LightScence();
    Talk(50, "少侠刚才似乎提到有许多事还没解决，为此烦恼不已，不知是什么事．", "talkname50", 0);
    Talk(0, "说来话长，总之，我现正在找江湖中人传说的”十四天书”．", "talkname0", 1);
    Talk(50, "原来是这档事，看来应该很棘手吧．", "talkname50", 0);
    Talk(0, "是啊．", "talkname0", 1);
    Talk(50, "这事别担心，书总会找齐的嘛，或许将来我乔峰也能出点力也说不定．", "talkname50", 0);
    Talk(0, "有乔帮主帮忙，我就放心多了．那么，在下就先行告退了．", "talkname0", 1);
    Talk(50, "慢走．", "talkname50", 0);
    ModifyEvent(-2, -2, -2, -2, 526, -1, -1, -2, -2, -2, -2, -2, -2);
do return end;
