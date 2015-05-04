using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

/// <summary>
/// Provides an means for events to be setup in editor and then translated for
/// use by the GameManager singleton.
/// </summary>
public class EventAdapter : MonoBehaviour
{
	public void DoneEditText(string txt)
	{
		if(!GameManager.IsInstanceNull)
		{
			GameManager.Instance.EnterText(txt);
			EventSystem.current.currentSelectedGameObject.
								GetComponentInChildren<InputField>().text = @"";
		}
	}

	#region room 0
	public void LookComputer(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("On the screen you see an email " +
	         "that you'd recently opened with the subject line: \"Cover " +
	         "letter and resume\".  You begin reading:\nDear Sir/Madam,\n" +
	         "\tI am writing to inform you that I am the perfect candidate " +
	         "for the position you posted.  I am desparate for a job.  " +
	         "Please hire me.  Pleeeeeaaasssse hire me.  I'm literally " +
	         "begging you...\n\nYour eyes glaze over and you see the " +
	         "dystopic future to be where you have to read this to " +
	         "completetion.  Is this hell?  You decide it's time to go to " +
	         "the supply closet.  Maybe this time you'll sort the pens by " +
	         "weight instead of color!\n");
	}
	#endregion

	#region room 1
	public void LookBoard(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("You peer at the board, wondering " +
			"who even posts on these things anymore.  On it, you see SELLING " +
			"POINT 1.  You TAKE it.\n");
		GameManager.Instance.PutInInventory(0);
	}

	public void TalkBoard(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("You scream, \"WHY DO YOU EVEN " +
			"EXIST?  WHAT IS YOUR PURPOSE?\" at the board.  \nAfter a few " +
			"seconds a far away voice sounding like clove cigarettes " +
			"responds, \"Why do any of us exist?  What a question.  " +
			"The inherint conflict of his existence drives a man to his " +
			"greatest heights as surely as it drives him mad\".\nYeah, I " +
			"don't know what sort of people you're hiring either.\n");
	}

	public void LookPoster(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("You take a closer look at one of " +
			"the posters.  It contains a cute little kitten hanging from a " +
			"tree branch.  When you examine the noose, you become dizzy " +
			"while green-ish blurs swim in and out of your field of vision.  " +
			"You feel a weight in your pocket.\n");
		GameManager.Instance.PutInInventory(1);
	}

	public void TalkPoster(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("You tell the kitten that the " +
			"workmanship on it's noose is sloppy.  The kitty says, \"Mew,\" " +
			"in response.\n");
	}

	public void AttackPoster(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("No!  Bad!  Don't attack the " +
			"kitty!  It has clearly suffered enough.\n");
	}
	#endregion

	#region room 2
	public void LookRefrigerator(BaseEventData bed)
	{
		if(GameManager.Instance.IsItemVisible("SLIME"))
		{
			GameManager.Instance.DisplayText("You see a SLIME in the " +
				"regrigerator.\n");
		}
		else if(!GameManager.Instance.IsItemVisible("MOLD") &&
			GameManager.Instance.IsItemTakeable("SLIME"))
		{
			GameManager.Instance.DisplayText("You open the refrigerator door " +
			                                 "and see...something.  Ewww.\n");
			GameManager.Instance.SetItemVisible("MOLD", true);
			GameManager.Instance.SetItemTakeable("MOLD", true);
		}
		else if(GameManager.Instance.IsItemVisible("MOLD"))
		{
			GameManager.Instance.DisplayText("You can see MOLD on " +
				"something.\n");
		}
		else
		{
			GameManager.Instance.DisplayText("The refrigerator is empty " +
			                                 "and safe...for now.\n");
		}
	}

	public void LookMold(BaseEventData bed)
	{
		if(GameManager.Instance.IsItemVisible("MOLD"))
		{
			GameManager.Instance.DisplayText("You can't even tell what this " +
				"mold started as.  It's just fuzzy looking and vaguely " +
                "intimidating.\n");
		}
	}

	public void TakeMold(BaseEventData bed)
	{
		if(GameManager.Instance.IsItemTakeable("MOLD"))
		{
			GameManager.Instance.DisplayText("You reach to take the mold.  " +
					"It growls and waves a miniature switchblade at you.\n");
		}
	}

	public void AttackMold(BaseEventData bed)
	{
		if(GameManager.Instance.IsItemVisible("MOLD") &&
		   GameManager.Instance.IsItemInInventory(5))
		{
			GameManager.Instance.DisplayText("You grab Comrade Bic from your " +
				"pocket and consider the mold.  A red haze descends on your " +
				"field of vision as you begin stabbing the mold with your " +
				"pen.  You can feel the mold fight back; you know that cuts " +
				"from it's knife will hurt tomorrow.  But today is today and " +
				"that mold needs to be taught a lesson.  You finish your " +
				"assault on the mold.  It's been reduced to a SLIME.  " +
				"Meanwhile, your pen has been damaged beyond repair.  " +
				"Normally this would infuriate you, but you know that " +
				"sometimes sacrifices must be made.  You say a quick prayer " +
				"in honor of your fallen comrade Bic.\n");
			GameManager.Instance.SetItemVisible("SLIME", true);
			GameManager.Instance.SetItemTakeable("SLIME", true);
			GameManager.Instance.SetItemVisible("MOLD", false);
			GameManager.Instance.SetItemTakeable("MOLD", false);
			GameManager.Instance.TakeFromInventory(5);
		}
		else if(GameManager.Instance.IsItemVisible("MOLD"))
		{
			GameManager.Instance.DisplayText("The mold appears too dangerous " +
														"to attack unarmed.\n");
		}
	}

	public void LookSlime(BaseEventData bed)
	{
		if(GameManager.Instance.IsItemVisible("SLIME"))
		{
			GameManager.Instance.DisplayText("The slime oozes along the " +
				"shelf in the refrigerator.  It smells more foul than the " +
				"mold, but you smile knowing that Comrade Bic's sacrifice " +
				"was noble and his memory shall endure for generations.\n");
		}
	}

	public void TakeSlime(BaseEventData bed)
	{
		if(GameManager.Instance.IsItemVisible("SLIME"))
		{
			GameManager.Instance.DisplayText("You find a container of " +
				"someone's food in the refrigerator and empty it into the " +
				"sink.  They'll never notice.  You scoop as much of the " +
				"slime into the container as possible and take the" +
				"container.\n");
			GameManager.Instance.PutInInventory(2);
			GameManager.Instance.SetItemTakeable("SLIME", false);
			GameManager.Instance.SetItemVisible("SLIME", false);
		}
	}
	#endregion

	#region room 3
	public void LookWindow(BaseEventData bed)
	{
		if(!GameManager.Instance.IsItemInInventory(3))
		{
			GameManager.Instance.DisplayText("You look out over the parking " +
				"lot and are startled to see a giant ear of corn dancing " +
				"with a sign.  Maybe it's a person dressed as a giant ear of " +
				"corn?  You immediately dismiss the thought as too weird.  " +
				"You try to make out the shifting letters on the sign and " +
				"SELLING POINT 4 appears in your hand.\n");
			GameManager.Instance.PutInInventory(3);
		}
		else
		{
			GameManager.Instance.DisplayText("You search for the giant ear " +
				"of corn but can no longer see it.  You shake your head and " +
				"chuckle.  A person dressed up as corn, what a silly idea.\n");
		}
	}
	#endregion

	#region room 4
	public void LookSink(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("It's a sink, man.  Just a normal " +
		                                 "sink.  Has faucets.  And a drain.\n");
	}

	public void UseSink(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("As you finish washing your hands " +
								"you notice a weird glimmer on the MIRROR.\n");
		GameManager.Instance.SetItemVisible("MIRROR", true);
	}

	public void LookMirror(BaseEventData bed)
	{
		if(GameManager.Instance.IsItemVisible("MIRROR") &&
		   !GameManager.Instance.IsItemInInventory(5) &&
		   !GameManager.Instance.IsItemInInventory(2))
		{
			GameManager.Instance.DisplayText("The glass of the mirror shifts " +
				"around with an unusual liquidity.  Beyond you can see your " +
				"reflection beckoning\n");
		}
		else if(GameManager.Instance.IsItemInInventory(5))
		{
			GameManager.Instance.DisplayText("Your reflection has not come" +
			                                 "back.\n");
		}
	}

	public void UseMirror(BaseEventData bed)
	{
		if(GameManager.Instance.IsItemVisible("MIRROR") &&
		   !GameManager.Instance.IsItemInInventory(5) &&
		   !GameManager.Instance.IsItemInInventory(2))
		{
			GameManager.Instance.DisplayText("You reach your hand to the " +
				"mirror and feel only a mild surprise as your hand reaches " +
				"right through it.  As it gazes into your eyes your " +
				"reflection reaches out and places something in your hand.  " +
				"Then, visibly weeping, your mirror-self turns and walks " +
				"away.\n");
			GameManager.Instance.PutInInventory(5);
		}
	}

	public void TalkMirror(BaseEventData bed)
	{
		if(GameManager.Instance.IsItemVisible("MIRROR") &&
		   !GameManager.Instance.IsItemInInventory(5) &&
		   !GameManager.Instance.IsItemInInventory(2))
		{
			GameManager.Instance.DisplayText("\"Hello,\" you say.\nYour " +
				"reflection responds, but you can't hear what he says.\n" +
				"\"What?\"\nThe reflection attempts to respond again.  As " +
				"well as speaking he points to his ears and shakes his " +
				"head.\n\"I CAN'T HEAR YOU\"\nAt this point your reflection " +
				"just shakes his head.  He makes a large gesture with his " +
				"hands, clearly inviting you to come inside the mirror.\n");
		}
	}
	#endregion

	#region room 5
	public void LookGnomes(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("When you stare into the garden " +
									"gnomes, the gnomes stare back at you.\n");
	}

	public void UseGnomes(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("Uh, gross.\n");
	}

	public void TalkGnomes(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("As one the gnomes speak, \"We have " +
			"been watching you\".\n\"Well that's just how you do, right?\"\n" +
			"They all nod.  \"Indeed, we are the ones who watch while " +
			"decorating.  We are tasteful and we are kitsch.  We are " +
			"obsession.  We are decor!\"\nAnd you nod, recalling a lesson " +
			"from the Ornamentation Monks of Dresden: \"The truest decoration" +
			"is both refined and ornate, possessing a harmony within itself " +
			"that spreads to all it adorns\".\n");
	}

	public void TakeGnomes(BaseEventData bed)
	{
		if(GameManager.Instance.IsItemTakeable("GNOMES"))
		{
			GameManager.Instance.DisplayText("You pick up one of the " +
						"gnomes.  It does not protest, nor do it's fellows.\n");
			GameManager.Instance.PutInInventory(4);
			GameManager.Instance.SetItemTakeable("GNOMES", false);
		}
	}
	#endregion

	#region room 6
	public void LookPeter(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("He stands before you, patiently " +
		"waiting for something.  A verdict on his employability, perhaps?\n");
	}
	public void TalkPeter(BaseEventData bed)
	{
		//unnecessary lambda because I've been watching too many lectures on
		//functional programming recently.
		Func<int, bool> haveAllSellingPoints = null;
		haveAllSellingPoints = (i) => {
			if(i < 4)
			{
				return GameManager.Instance.IsItemInInventory(i) &&
						haveAllSellingPoints(i + 1);
			}
			else
			{
				return GameManager.Instance.IsItemInInventory(4);
			}
		};

		if(haveAllSellingPoints(0))
		{
			GameManager.Instance.DisplayText("\"I see you have gathered all of " +
				"the SELLING POINTs.  I would appreciate it if you would " +
				"LOOK at each of them if you have not already done so.  " +
				"Thank you for playing along with me in this cover letter " +
				"game.  I hope you had as much fun with the details as I had " +
				"making them.  I can be reached at bartoschp@gmail.com or " +
				"608-616-0192.  Below the game window are links to each of " +
				"the repos mentioned in the SELLING POINTs.\"\n");
		}
		else
		{
			GameManager.Instance.DisplayText("\"Hmmm.  It appears you do not " +
				"have all five SELLING POINTs.  I choose to believe this is " +
				"because you haven't had the opportunity to fully explore " +
				"yet.  If you do not have the time to complete the cover " +
				"letter game I can be reached at bartoschp@gmail.com or " +
				"608-616-0192.  Below the game window are links to each of " +
				"the repos mentioned in the SELLING POINTs.\"\n");
		}
	}

	public void AttackPeter(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("\"Ouch\"\n");
	}

	public void UsePeter(BaseEventData bed)
	{
		GameManager.Instance.DisplayText("\"I assume this means that you " +
			"wish to skip the rest of the interview process and hire me.  " +
			"I can be reached at bartoschp@gmail.com or 608-616-0192.  " +
			"Below the game window are links to each of the repos mentioned " +
			"in the SELLING POINTs.\"\n");
	}
	#endregion
}
