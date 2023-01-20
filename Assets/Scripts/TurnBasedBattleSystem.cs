using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class TurnBasedBattleSystem : MonoBehaviour
{

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Monsters playerUnit;
	Monsters enemyUnit;

	public Text dialogueText;

	public BattleHuds playerHUD;
	public BattleHuds enemyHUD;

	public BattleState state;
	public Animator enemyAnimator;
	public Animator playerAnimator;
	// Start is called before the first frame update
	void Start()
	{
		state = BattleState.START;
		StartCoroutine(SetupBattle());
		playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
		enemyAnimator = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>();
	}

	IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
		playerUnit = playerGO.GetComponent<Monsters>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
		enemyUnit = enemyGO.GetComponent<Monsters>();

		dialogueText.text = "A wild " + enemyUnit.monsterName + " approaches...";

		playerHUD.UpdateHud(playerUnit);
		enemyHUD.UpdateHud(enemyUnit);

		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	IEnumerator PlayerAttack()
	{
		bool isDead = enemyUnit.TakeDamage(playerUnit.monsterDamage);
		if (isDead)
		{
			state = BattleState.WON;
			enemyHUD.UpdateHP(enemyUnit.monsterCurrentHP = 0);
			enemyAnimator.SetBool("isEnemyDeath", true);
			EndBattle();
			yield return new WaitForSeconds(1.5f);
			Time.timeScale = 0;
		}
		else
		{ 
			state = BattleState.ENEMYTURN;
			playerAnimator.SetBool("isAttacking", true);
			enemyHUD.UpdateHP(enemyUnit.monsterCurrentHP);
			dialogueText.text = playerUnit.monsterName + " Deals " + playerUnit.monsterDamage + " damage " + " TO " + enemyUnit.monsterName;
			yield return new WaitForSeconds(2f);
			StartCoroutine(EnemyTurn());
			playerAnimator.SetBool("isAttacking", false);
			yield return new WaitForSeconds(1f);
			enemyAnimator.SetBool("isEnemyGetHit", true);
			yield return new WaitForSeconds(1f);
			enemyAnimator.SetBool("isEnemyGetHit", false);
		}
	}

	IEnumerator EnemyTurn()
	{
		dialogueText.text = enemyUnit.monsterName + " Deals " + enemyUnit.monsterDamage + " damage " + " To "+playerUnit.monsterName;

		yield return new WaitForSeconds(1f);
		enemyAnimator.SetBool("isEnemyAttack", true);
		bool isDead = playerUnit.TakeDamage(enemyUnit.monsterDamage);
		
		yield return new WaitForSeconds(2f);
		playerAnimator.SetBool("isGetHit", true);
		
		yield return new WaitForSeconds(2f);
		enemyAnimator.SetBool("isEnemyAttack", false);
		playerHUD.UpdateHP(playerUnit.monsterCurrentHP);

		yield return new WaitForSeconds(2f);
		playerAnimator.SetBool("isGetHit", false);

		if (isDead)
		{
			state = BattleState.LOST;
			playerAnimator.SetBool("isDead", true);
			EndBattle();
		}
		else
		{
			state = BattleState.PLAYERTURN;
			PlayerTurn();
		}

	}

	void EndBattle()
	{
		if (state == BattleState.WON)
		{
			dialogueText.text = enemyUnit.monsterName + " Fainted";
		}
		else if (state == BattleState.LOST)
		{
			dialogueText.text = playerUnit.monsterName+ " Fainted";
		}
	}

	void PlayerTurn()
	{
		dialogueText.text = "Fight or Fly ? ";
	}

	IEnumerator PlayerHeal()
	{
		playerUnit.Heal(5);
		state = BattleState.ENEMYTURN;
		playerHUD.UpdateHP(playerUnit.monsterCurrentHP);
		dialogueText.text = playerUnit.monsterName + " healed 5 HP";

		yield return new WaitForSeconds(2f);

		
		StartCoroutine(EnemyTurn());
	}

	public void OnAttackButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerAttack());
	}

	public void OnHealButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerHeal());
	}

}