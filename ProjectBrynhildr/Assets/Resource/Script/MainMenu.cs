using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour 
{
	public void StartGameClicked()
	{
		
		LoadingManager.Instance.SetSceneToUnload (SceneNames.MAIN_MENU);
		LoadingManager.Instance.SetSceneToLoad (SceneNames.GAME_UI + "," + SceneNames.GAME_SCENE);
		LoadingManager.Instance.LoadGameScene ();

		//Okay_Click ();
	}

	private void Okay_Click(object sender = null)
	{
		#region
		string fx = "2x+3";
		string gx = "-32x+2";

		bool isBinomial = PolynomialChecker(fx);
		isBinomial = PolynomialChecker(gx);

		if (isBinomial)
		{
			Debug.Log("Valid Input");
			string temp = "";
			string ans = "";
			for(int i = 0; i < fx.Length; i++)
			{
				if (fx[i] == 'x')
				{
					int number;
					int.TryParse(temp, out number);
					ans = ans + SolveMultiplication(number, gx);
					Debug.Log(ans);

					temp = temp + "*(" + gx + ")";
				}
				else if (fx[i] == '+' || fx[i] == '-')
				{
					temp = temp + fx[i];
				}
				else if (fx[i] >= '0' && fx[i] <= '9') 
				{
					temp = temp + fx[i];
				}else
				{
					Debug.Log("Invalid Input");
					break;
				}
				Debug.Log(temp);
			}
		}

		else 
		{
			Debug.Log("Invalid Input");
		}
	} 
		#endregion

	private bool PolynomialChecker(string binomial)
	{
		char[] symbol = {'+', '-'};
		for(int i = 0; i < symbol.Length; i++)
		{
			string[] tempBinomial = binomial.Split (symbol[i]);
			if(tempBinomial.Length > 1)
				return true;
		}
		return false;
	}

	private string SolveMultiplication(int num, string stringToParse)
	{
		if (num == 0)
			return "";

		string newString = "";
		char[] symbol = {'+', '-'};
		for (int i = 0; i < symbol.Length; i++)
		{
			char selectedSymbol = symbol [i];
			char[] tempBinomial = stringToParse.ToCharArray ();
			if (stringToParse.Split (symbol[i]).Length > 1) 
			{
				string toSolve = "";
				for (int k = 0; k < tempBinomial.Length; k++) 
				{
					if (char.IsDigit (tempBinomial [k]) || tempBinomial [k] == '-') 
					{
						toSolve += tempBinomial [k];
					} 
					else if(!char.IsDigit (tempBinomial [k]))
					{
						if (toSolve != "") 
						{
							int charNum = new int();
							if (int.TryParse (toSolve.ToString (), out charNum)) 
							{
								charNum = num + charNum;
								newString += charNum.ToString () + tempBinomial [k];
								toSolve = "";
							}
						}
						else
							newString += tempBinomial [k].ToString ();
					}

					if(k + 1 == tempBinomial.Length)
					{
						if (toSolve != "") 
						{
							int charNum = new int();
							if (int.TryParse (toSolve.ToString (), out charNum))
							{
								charNum = num * charNum;
								newString += charNum.ToString ();
								toSolve = "";
							}
						}
					}
				}
			}
		}
		return newString;
	}
}
