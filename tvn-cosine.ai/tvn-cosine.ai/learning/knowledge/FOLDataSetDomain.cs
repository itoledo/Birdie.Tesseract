 namespace aima.core.learning.knowledge;

 
 
 
 
 

import aima.core.learning.framework.DataSetSpecification;
import aima.core.logic.fol.domain.FOLDomain;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class FOLDataSetDomain : FOLDomain {
	//
	private static Pattern allowableCharactersRegEx = Pattern
			.compile("[^a-zA-Z_$0-9]");
	//
	private DataSetSpecification dataSetSpecification;
	private string trueGoalValue = null;
	// Default example prefix, see pg679 of AIMA
	private string examplePrefix = "X";
	private List<string> descriptionPredicateNames = new List<string>();
	private List<string> descriptionDataSetNames = new List<string>();
	private IDictionary<String, String> dsToFOLNameMap = new Dictionary<String, String>();

	//
	// PUBLIC METHODS
	//
	public FOLDataSetDomain(DataSetSpecification dataSetSpecification,
			String trueGoalValue) {
		this.dataSetSpecification = dataSetSpecification;
		this.trueGoalValue = trueGoalValue;
		constructFOLDomain();
	}

	public string getDataSetTargetName() {
		return dataSetSpecification.getTarget();
	}

	public string getGoalPredicateName() {
		return getFOLName(dataSetSpecification.getTarget());
	}

	public string getTrueGoalValue() {
		return trueGoalValue;
	}

	public List<string> getDescriptionPredicateNames() {
		return descriptionPredicateNames;
	}

	public List<string> getDescriptionDataSetNames() {
		return descriptionDataSetNames;
	}

	public bool isMultivalued(string descriptiveDataSetName) {
		List<string> possibleValues = dataSetSpecification
				.getPossibleAttributeValues(descriptiveDataSetName);
		// If more than two possible values
		// then is multivalued
		if (possibleValues.Count > 2) {
			return true;
		}
		// If one of the possible values for the attribute
		// matches the true goal value then consider
		// it not being multivalued.
		for (string pv : possibleValues) {
			if (trueGoalValue .Equals(pv)) {
				return false;
			}
		}

		return true;
	}

	public string getExampleConstant(int egNo) {
		String egConstant = examplePrefix + egNo;
		addConstant(egConstant);
		return egConstant;
	}

	public string getFOLName(string dsName) {
		String folName = dsToFOLNameMap.get(dsName);
		if (null == folName) {
			folName = dsName;
			if (!Character.isJavaIdentifierStart(dsName.charAt(0))) {
				folName = "_" + dsName;
			}
			folName = allowableCharactersRegEx.matcher(folName).replaceAll("_");
			dsToFOLNameMap.Add(dsName, folName);
		}

		return folName;
	}

	//
	// PRIVATE METHODS
	//
	private void constructFOLDomain() {
		// Ensure the target predicate is included
		addPredicate(getFOLName(dataSetSpecification.getTarget()));
		// Create the descriptive predicates
		for (string saName : dataSetSpecification.getNamesOfStringAttributes()) {
			if (dataSetSpecification.getTarget() .Equals(saName)) {
				// Don't add the target to the descriptive predicates
				continue;
			}
			String folSAName = getFOLName(saName);
			// Add a predicate for the attribute
			addPredicate(folSAName);

			descriptionPredicateNames.Add(folSAName);
			descriptionDataSetNames.Add(saName);

			List<string> attributeValues = dataSetSpecification
					.getPossibleAttributeValues(saName);
			// If a multivalued attribute need to setup
			// Constants for the different possible values
			if (isMultivalued(saName)) {
				for (string av : attributeValues) {
					addConstant(getFOLName(av));
				}
			}
		}
	}
}
