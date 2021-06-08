using System;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;

namespace Umbraco.Cms.Infrastructure.Migrations.Upgrade
{
    /// <summary>
    /// Represents an upgrader.
    /// </summary>
    public class Upgrader
    {
        /// <summary>
        /// Initializes a new instance of the <see ref="Upgrader"/> class.
        /// </summary>
        public Upgrader(MigrationPlan plan)
        {
            Plan = plan;
        }

        /// <summary>
        /// Gets the name of the migration plan.
        /// </summary>
        public string Name => Plan.Name;

        /// <summary>
        /// Gets the migration plan.
        /// </summary>
        public MigrationPlan Plan { get; }

        /// <summary>
        /// Gets the key for the state value.
        /// </summary>
        public virtual string StateValueKey => "Umbraco.Core.Upgrader.State+" + Name;

        /// <summary>
        /// Executes.
        /// </summary>
        /// <param name="scopeProvider">A scope provider.</param>
        /// <param name="keyValueService">A key-value service.</param>
        public void Execute(IMigrationPlanExecutor migrationPlanExecutor, IScopeProvider scopeProvider, IKeyValueService keyValueService)
        {
            if (scopeProvider == null) throw new ArgumentNullException(nameof(scopeProvider));
            if (keyValueService == null) throw new ArgumentNullException(nameof(keyValueService));

            var plan = Plan;

            using (var scope = scopeProvider.CreateScope())
            {
                // read current state
                var currentState = keyValueService.GetValue(StateValueKey);
                var forceState = false;

                if (currentState == null)
                {
                    currentState = plan.InitialState;
                    forceState = true;
                }

                // execute plan
                var state = migrationPlanExecutor.Execute(plan, currentState);
                if (string.IsNullOrWhiteSpace(state))
                {
                    throw new Exception("Plan execution returned an invalid null or empty state.");
                }

                // save new state
                if (forceState)
                {
                    keyValueService.SetValue(StateValueKey, state);
                }
                else if (currentState != state)
                {
                    keyValueService.SetValue(StateValueKey, currentState, state);
                }

                scope.Complete();
            }
        }
    }
}
