using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanBeh.Models
{
    public class NodeData
    {
        NodeData()
        {
            NodeColorHexes.Add("#ba3946"); //CONDITIONAL
            NodeColorHexes.Add("#ff7f50"); //ACTION
            NodeColorHexes.Add("#47848e"); //SEQUENCE
            NodeColorHexes.Add("#2e4c50"); //SELECTOR
            NodeColorHexes.Add("#021a3f"); //PARTIALSEQUENCE
            NodeColorHexes.Add("#a3c99a"); //PARTIALSELECTOR
            NodeColorHexes.Add("#f9e76c"); //PARALLEL
            NodeColorHexes.Add("#e497ab"); //PRIORITYLIST
            NodeColorHexes.Add("#5f631d"); //RANDOMSELECTOR
            NodeColorHexes.Add("#359797"); //RANDOMSEQUENCE

            NodeTypeDescriptions.Add("A conditional checks whether a certain condition has been met or not."); //CONDITIONAL
            NodeTypeDescriptions.Add("Action nodes perform computations to change the agent state."); //ACTION
            NodeTypeDescriptions.Add("The sequence node ticks its children sequentially until one of them returns FAILURE, RUNNING or ERROR. If all children return the success state, the sequence also returns SUCCESS."); //SEQUENCE
            NodeTypeDescriptions.Add("The selector ticks its children sequentially until one of them returns SUCCESS, RUNNING or ERROR. If all children return the failure state, the priority also returns FAILURE."); //SELECTOR
            NodeTypeDescriptions.Add("Same as the normal sequence except it starts from the previous RUNNING return."); //PARTIALSEQUENCE
            NodeTypeDescriptions.Add("Same as the normal selector except it starts from the previous RUNNING return."); //PARTIALSELECTOR
            NodeTypeDescriptions.Add("The parallel node ticks all children at the same time, allowing them to work in parallel."); //PARALLEL
            NodeTypeDescriptions.Add("???Possibly a list of selectors???"); //PRIORITYLIST
            NodeTypeDescriptions.Add("Selector that runs its children in a random order."); //RANDOMSELECTOR
            NodeTypeDescriptions.Add("Sequence that runs its children in a random order."); //RANDOMSEQUENCE

        }
        public List<string> NodeColorHexes { get; set; }
        public List<string> NodeTypeDescriptions { get; set; }
    }
}
