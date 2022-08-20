﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TodoSynchronizer.Core.Config;
using TodoSynchronizer.Core.Models.CanvasModels;

namespace TodoSynchronizer.Core.Helpers
{
    public static class CanvasStringTemplateHelper
    {
        public static string GetTitle(Course course, ICanvasItem item)
        {
            if (item is Assignment assignment)
            {
                return SyncConfig.Default.AssignmentConfig.TitleTemplate
                    .ReplaceCourse(course)
                    .Replace("{assignment.title}", assignment.Title);
            }
            if (item is Anouncement anouncement)
            {
                return SyncConfig.Default.AnouncementConfig.TitleTemplate
                    .ReplaceCourse(course)
                    .Replace("{anouncement.title}", anouncement.Title)
                    .Replace("{anouncement.author}", anouncement.Author.DisplayName);
            }
            if (item is Quiz quiz)
            {
                return SyncConfig.Default.QuizConfig.TitleTemplate
                    .ReplaceCourse(course)
                    .Replace("{quiz.title}", quiz.Title);
            }
            if (item is Discussion discussion)
            {
                return SyncConfig.Default.DiscussionConfig.TitleTemplate
                    .ReplaceCourse(course)
                    .Replace("{discussion.title}", discussion.Title);
            }
            return "Error";
        }

        public static string GetListNameForCourse(Course course)
        {
            return SyncConfig.Default.ListNameTemplateForCourse.ReplaceCourse(course);
        }

        public static string ReplaceCourse(this string s, Course course)
        {
            return s.Replace("{course.name}", course.Name)
                .Replace("{course.coursecode}", course.CourseCode)
                .Replace("{course.coursecodeshort}", ExtractCourseCodeShort(course.CourseCode))
                .Replace("{course.originalname}", course.OriginalName ?? course.Name);
        }

        public static string ExtractCourseCodeShort(string s)
        {
            var reg = new Regex(@"\(\d{4}-\d{4}-[123]\)-([a-zA-z0-9]+?)-");
            var match = reg.Match(s);
            return match.Success ? match.Groups[1].Value : s;
        }

        public static string GetContent(ICanvasItem item)
        {
            HtmlHelper convert = new HtmlHelper();
            return convert.Convert(item.Content);
        }

        public static string GetSubmissionDesc(Assignment assignment, AssignmentSubmission submission)
        {
            if (submission.SubmittedAt != null)
                return $"已提交（提交时间：{submission.SubmittedAt.Value.ToString("yyyy-MM-dd HH:mm:ss")}）";
            else
                return "未提交";
        }

        public static string GetGradeDesc(Assignment assignment, AssignmentSubmission submission)
        {
            if (submission.Grade != null)
                return $"已评分：{submission.Grade}/{assignment.PointsPossible}（评分时间：{submission.GradedAt.Value.ToString("yyyy-MM-dd HH:mm:ss")}）";
            else
                return "未评分";
        }

        public static string GetSubmissionDesc(Assignment assignment, QuizSubmission submission)
        {
            return $"尝试 {submission.Attempt}：{submission.Score}/{submission.QuizPointsPossible}（提交时间：{submission.FinishedAt.ToString("yyyy-MM-dd HH:mm:ss")}）";
        }
    }
}
