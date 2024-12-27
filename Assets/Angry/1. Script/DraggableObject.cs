// 이 스크립트는 Unity 씬에서 객체를 드래그하는 메커니즘을 처리합니다.
// 포인터 동작(클릭, 드래그, 해제)에 응답하기 위해 Unity의 이벤트 인터페이스를 구현합니다.
// LineRenderer를 사용하여 당기는 방향을 시각화하고 드래그 거리를 제한합니다.

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, 
    IPointerDownHandler,  // 포인터 클릭 이벤트를 처리합니다.
    IPointerUpHandler,    // 포인터 해제 이벤트를 처리합니다.
    IDragHandler          // 드래그 이벤트를 처리합니다.
{ 
    //사운드
    
    
    // 초기 위치와 현재 당기는 위치를 저장하는 변수입니다.
    private Vector3 startPosition;
    private Vector3 pullPosition;
    
    // 화면 좌표를 월드 좌표로 변환하기 위해 메인 카메라를 참조합니다.
    private Camera MainCamera;
    [SerializeField] private LineRenderer leftLineRenderer; // 왼쪽 새총 끝
    [SerializeField] private LineRenderer rightLineRenderer; // 오른쪽 새총 끝
    [SerializeField] private float maxPullDistance;     // 허용되는 최대 당김 거리입니다.

    public GameObject MetalPrefab; // 발사체로 사용할 오브젝트 프리팹입니다.
    public float speed = 100f;     // 발사체의 발사 속도를 조절하는 변수입니다.

    public GameObject TrajectoryPrefab; // 궤적을 표시할 오브젝트 프리팹입니다.
    private List<GameObject> trajectoryObjects = new List<GameObject>(); // 궤적 오브젝트 리스트
    public int maxTrajectorySteps = 20; // 궤적의 최대 스텝 수

    // 각 시뮬레이션 스텝 간의 시간 간격 (s)
    public float timeStep = 0.1f;
    

    private GameOverCheck _gameOverCheck;
    // 스크립트 인스턴스가 로드될 때 호출됩니다.
    private void Awake()
    {
        MainCamera = Camera.main; // 메인 카메라를 참조합니다.
        _gameOverCheck = FindObjectOfType<GameOverCheck>();
    }

    // 객체가 클릭되었을 때 호출됩니다.
    public void OnPointerDown(PointerEventData eventData)
    {
        // 포인터 위치를 기준으로 월드 좌표에서 시작 위치를 기록합니다.
        pullPosition = startPosition = MainCamera.ScreenToWorldPoint(
            new Vector3(eventData.position.x, 
                eventData.position.y, 
                MainCamera.WorldToScreenPoint(transform.position).z));
    }

    // 포인터가 해제되었을 때 호출됩니다.
    public void OnPointerUp(PointerEventData eventData)
    {
        // 새총 중심 위치 계산 (왼쪽과 오른쪽 끝점의 중간)
        Vector3 leftPoint = leftLineRenderer.GetPosition(0);
        Vector3 rightPoint = rightLineRenderer.GetPosition(0);
        Vector3 start = (leftPoint + rightPoint) / 2; // 중심점

        // 발사 방향 계산
        Vector3 powerDirection = start - transform.position;

        // 발사체 생성 및 발사
        GameObject projectile = Instantiate(MetalPrefab, start, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 force = powerDirection.normalized * speed * powerDirection.magnitude;
            rb.AddForce(force, ForceMode.Impulse);
        }

        // 카메라 발사체 따라가기 
        projectileScript.Launch();
   
        _gameOverCheck.FireProjectile();
    
  
        // 궤적 오브젝트 초기화
        ClearTrajectory();
        transform.position = startPosition;
        leftLineRenderer.SetPosition(1, startPosition);
        rightLineRenderer.SetPosition(1, startPosition);
    }

    // 객체를 드래그하는 동안 지속적으로 호출됩니다.
    public void OnDrag(PointerEventData eventData)
    {
       
        if (Camera.main != null)
        {
            // 포인터 위치를 월드 좌표로 변환
            Vector3 mouseWorldPos = MainCamera.ScreenToWorldPoint(
                new Vector3(eventData.position.x, 
                eventData.position.y, 
                MainCamera.WorldToScreenPoint(transform.position).z));
            
            // 당기는 방향 계산
            Vector3 pullDirection = startPosition - mouseWorldPos;

            // 최대 당김 거리 제한
            if (pullDirection.magnitude > maxPullDistance)
            {
                pullDirection = pullDirection.normalized * maxPullDistance;
            }

            // 드래그 위치 업데이트
            transform.position = startPosition - pullDirection;

            // LineRenderer 업데이트
            UpdateLineRenderers(transform.position);

            // 궤적 업데이트
            UpdateTrajectory(transform.position);
            AudioManager.Instance.PlaySound(5); 
        }
    }

    // LineRenderer를 업데이트합니다.
    private void UpdateLineRenderers(Vector3 launchPosition)
    {
        // LineRenderer의 시작점과 끝점을 업데이트
        leftLineRenderer.SetPosition(1, launchPosition);
        rightLineRenderer.SetPosition(1, launchPosition);
    }

    // 궤적을 업데이트
    private void UpdateTrajectory(Vector3 launchPosition)
    {
        ClearTrajectory(); // 이전 궤적 삭제

        // 새총 중심 위치 계산 (왼쪽과 오른쪽 끝점의 중간)
        Vector3 leftPoint = leftLineRenderer.GetPosition(0);
        Vector3 rightPoint = rightLineRenderer.GetPosition(0);
        Vector3 start = (leftPoint + rightPoint) / 2; // 중심점

        // 발사 방향 계산
        Vector3 powerDirection = start - launchPosition;

        // 초기 속도 계산
        Vector3 velocity = powerDirection.normalized * speed * powerDirection.magnitude;

        // 궤적 생성
        for (int i = 0; i < maxTrajectorySteps; i++)
        {
            float timeElapsed = timeStep * i;
            Vector3 newPosition = start + 
                                  velocity * timeElapsed + 
                                  Physics.gravity * (0.5f * timeElapsed * timeElapsed);

            GameObject trajectoryPoint = Instantiate(TrajectoryPrefab, newPosition, Quaternion.identity);
            trajectoryObjects.Add(trajectoryPoint);
        }
    }

    // 궤적을 초기화합니다.
    private void ClearTrajectory()
    {
        foreach (var obj in trajectoryObjects)
        {
            Destroy(obj);
        }
        trajectoryObjects.Clear();
    }
}
