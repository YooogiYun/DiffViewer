﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiffViewer.Models;

public static class TestCaseHelper
{
    public const string sIdentical = "are identical";
    public const string sSevereError1 = "No such file or directory";
    public const string sSevereError2 = "SEVERE ERROR";
    public const string sSevereError3 = "No dump file";
    public const string sPostProcessingString = "Post Processing...";

    static void IsNullTestCase(TestCase testCase)
    {
        if( testCase is null )
        {
            App.Logger.Error($"{testCase} is null");
            throw new NullReferenceException($"{testCase} is null");
        }
    }

    /// <summary>
    /// Set the test case's Raw property
    /// </summary>
    /// <param name="testCase"></param>
    /// <param name="raw"></param>
    public static TestCase SetRaw(this TestCase testCase , string raw)
    {
        IsNullTestCase(testCase);
        testCase.Raw = raw;
        return testCase;
    }

    /// <summary>
    /// Set the test case's NewText_Actual property
    /// </summary>
    /// <param name="testCase"></param>
    /// <param name="newText"></param>
    public static TestCase SetNewText(this TestCase testCase , string newText)
    {
        IsNullTestCase(testCase);
        testCase.NewText_Actual = newText;
        return testCase;
    }

    /// <summary>
    /// Set the test case's OldText_BaseLine property
    /// </summary>
    /// <param name="testCase"></param>
    /// <param name="oldText"></param>
    public static TestCase SetOldText(this TestCase testCase , string oldText)
    {
        IsNullTestCase(testCase);
        testCase.OldText_BaseLine = oldText;
        return testCase;
    }

    /// <summary>
    /// Set the test case's IsIdentical property
    /// If the text ends with "are identical", the test case is Identical   = true
    /// If not, the test case is not identical,                 IsIdentical = false
    /// If null, the test case is a severe error,               IsIdentical = null
    /// </summary>
    /// <param name="testCase"></param>
    /// <param name="identicalText"></param>
    public static TestCase SetIdentical(this TestCase testCase , string identicalText)
    {
        IsNullTestCase(testCase);
        if( identicalText.EndsWith(sIdentical) || identicalText.Contains(sIdentical) )
        {
            testCase.IsIdentical = true;
        }
        else if( identicalText.Contains(sSevereError1) || identicalText.Contains(sSevereError2) || identicalText.Contains(sSevereError3) )
        {
            testCase.IsIdentical = null;
        }
        else
        {
            testCase.IsIdentical = false;
        }
        return testCase;
    }

    /// <summary>
    /// Set the test case's MoreInfo property
    /// </summary>
    /// <param name="testCase"></param>
    /// <param name="testCaseShare"></param>
    public static void SetMoreInfo(this TestCase testCase , TestCaseShare testCaseShare)
    {
        testCase.MoreInfo = testCaseShare;
    }

    /// <summary>
    /// Set the test case's MoreInfo property with the same value
    /// </summary>
    /// <param name="list"></param>
    /// <param name="testCaseShare"></param>
    public static void SetMoreInfo(this List<TestCase> list , TestCaseShare testCaseShare)
    {
        Parallel.ForEach(list , (testCase) =>
        {
            testCase.MoreInfo = testCaseShare;
        });
    }

    /// <summary>
    /// Get the version number from the test case
    /// </summary>
    /// <param name="testCase"></param>
    /// <returns></returns>    
    public static int GetVersionTested(this TestCase testCase)
    {
        IsNullTestCase(testCase);
        return int.TryParse(testCase.MoreInfo?.Version , out var version) ? version : -1;
    }

    /// <summary>
    /// Get the media number from the test case
    /// </summary>
    /// <param name="testCase"></param>
    /// <returns></returns>
    public static int GetMediaTested(this TestCase testCase)
    {
        IsNullTestCase(testCase);
        return int.TryParse(testCase.MoreInfo?.Media , out var media) ? media : -1;
    }

    /// <summary>
    /// Get the date tested from the test case
    /// </summary>
    /// <param name="testCase"></param>
    /// <returns></returns>
    public static DateTime GetDateTested(this TestCase testCase)
    {
        IsNullTestCase(testCase);
        return DateTime.TryParse(testCase.MoreInfo?.Time , out var dateTested) ? dateTested : DateTime.MinValue;


    }

    /// <summary>
    /// Create a List<TestCase> only with the name thourgh the List<string>
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<TestCase> CreateTCswithName(this List<string> list)
    {
        List<TestCase> tcList = new List<TestCase>();
        for( int i = 0; i < list.Count; i++ )
        {
            tcList.Add(new TestCase() { Name = list[i] });
        }
        return tcList;
    }

}