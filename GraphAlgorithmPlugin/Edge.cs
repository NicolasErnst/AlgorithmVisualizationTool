using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public class Edge<T> : QuikGraph.Edge<T> where T : class, IVertex, new()
    {
        public Edge() : base(null, null)
        {
            // Nothing to do here
        }

        public Edge(T source, T target) : this(source, target, new Dictionary<string, string>())
        {
            // Nothing to do here
        }

        public Edge(T source, T target, IDictionary<string, string> properties) : base(source, target)
        {
            OnInitialize(properties);
        }

        public virtual void OnInitialize(IDictionary<string, string> properties)
        {
            // Nothing to do here, can be overwritten
        }
    }
}
