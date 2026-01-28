using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPerspective : MonoBehaviour
{
    public enum Perspective
    {
        First,
        Third
    };

    public enum ContainerName
    {
        Container1P,
        Container3P
    };

    [SerializeField]
    private Perspective perspective = Perspective.Third;

    Dictionary<Perspective, ContainerName> containerNameByPerspective = new Dictionary<Perspective, ContainerName>();
    Dictionary<ContainerName, Transform> containerByName = new Dictionary<ContainerName, Transform>();

    void Start()
    {
        InitialiseContainers();
        PerspectiveSwitch(perspective);
    }

    void InitialiseContainers()
    {
        containerNameByPerspective.Add(Perspective.First, ContainerName.Container1P);
        containerNameByPerspective.Add(Perspective.Third, ContainerName.Container3P);

        containerByName.Add(ContainerName.Container1P, gameObject.transform.Find(ContainerName.Container1P.ToString()));
        containerByName.Add(ContainerName.Container3P, gameObject.transform.Find(ContainerName.Container3P.ToString()));
    }

    void DisableAllContainers()
    {
        foreach (KeyValuePair<ContainerName, Transform> container in containerByName)
        {
            container.Value.gameObject.SetActive(false);
        }
    }

    void ActivatePerspective(Perspective perspective)
    {
        ContainerName container;
        if (containerNameByPerspective.TryGetValue(perspective, out container))
        {
            if (containerByName.TryGetValue(container, out Transform transform))
            {
                transform.gameObject.SetActive(true);
            }
        }
    }

    void PerspectiveSwitch(Perspective perspective)
    {
        DisableAllContainers();
        ActivatePerspective(perspective);
    }

    public Perspective GetPerspective()
    {
        return perspective;
    }

    public void SetPerspective(Perspective perspective)
    {
        if (this.perspective == perspective)
        {
            return;
        }

        this.perspective = perspective;

        PerspectiveSwitch(perspective);
    }
}