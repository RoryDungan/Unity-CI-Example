using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class AutoClickerTests
{
    private AutoClickerItem testObject;

    private void Init()
    {
        testObject = new AutoClickerItem();
    }

    [Test]
    public void returns_zero_initially()
    {
        Init();

        var value = testObject.Update(0f);

        Assert.That(value, Is.EqualTo(0));
    }

    [Test]
    public void returns_1_after_updating_past_time_threshold()
    {
        Init();

        var value = testObject.Update(0.5f);
        value += testObject.Update(1f);
        value += testObject.Update(1.5f);

        Assert.That(value, Is.EqualTo(1));
    }
}
