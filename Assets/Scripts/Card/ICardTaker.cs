using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface ICardTaker
{
    bool CanTakeCard(List<CardController> cardStacks);
    void TakeCard(List<CardController> cardStacks);
}
