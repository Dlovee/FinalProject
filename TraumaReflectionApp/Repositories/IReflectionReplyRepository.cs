using System.Collections.Generic;
using TraumaReflectionApp.Models;

public interface IReflectionReplyRepository
{
    void AddReply(ReflectionReply reply); 
    IEnumerable<ReflectionReply> GetRepliesByReflectionId(int reflectionId);
}